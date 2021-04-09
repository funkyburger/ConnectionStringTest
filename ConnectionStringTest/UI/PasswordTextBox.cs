using ConnectionStringTest.Data;
using ConnectionStringTest.EventHandling;
using ConnectionStringTest.Extensions;
using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public class PasswordTextBox : TextBox
    {
        private Keys lastKeyPressedBeforeTextChange = Keys.None;

        protected readonly IList<IEventHandler> Handlers;
        protected readonly IPasswordHelper PasswordHelper;
        protected readonly IHistoryService HistoryService;

        public string UnmaskedConnectionString { get; private set; }
        public StringHistoryStack stack { get; private set; }

        private bool lockTextChangedEvent = false;

        public PasswordTextBox(IPasswordHelper passwordHelper, IHistoryService historyService)
        {
            PasswordHelper = passwordHelper;
            HistoryService = historyService;

            UnmaskedConnectionString = string.Empty;
            Text = string.Empty;
            stack = new StringHistoryStack(string.Empty, 0, 0);

            Handlers = new List<IEventHandler>();

            AutoCompleteMode = AutoCompleteMode.Suggest;
            AutoCompleteSource = AutoCompleteSource.CustomSource;

            KeyPress += OnKeyPressedInternal;
            TextChanged += TextChangedInternal;
        }

        public void AddEventHandler(IEventHandler handler)
        {
            Handlers.Add(handler);
        }

        public void RefreshAutoComplete()
        {
            AutoCompleteCustomSource = HistoryService.GetAutoComplete() as AutoCompleteStringWithPasswordsCollection;
        }

        private async void TextChangedInternal(object sender, EventArgs e)
        {
            if (lockTextChangedEvent) 
            {
                return;
            };

            System.Diagnostics.Debug.WriteLine($"Text changed ('{Text}'). Selection : {SelectionStart}, {SelectionLength}");

            if (lastKeyPressedBeforeTextChange == Keys.None
                || lastKeyPressedBeforeTextChange == Keys.Down
                || lastKeyPressedBeforeTextChange == Keys.Up)
            {
                if (AutoCompleteCustomSource.Contains(Text))
                {
                    var passwordAutoComplete = AutoCompleteCustomSource as IAutoCompleteStringWithPasswordsCollection;
                    
                    ApplyTextChangeToUnmaskedString(new TextChange()
                    {
                        Beginning = 0,
                        End = -1,
                        Modification = passwordAutoComplete.GetUnmasked(Text)
                    });
                }
            }

            MaskPassword();

            foreach (var handler in Handlers)
            {
                await handler.Handle(Event.ConnectionStringBoxTextChanged, this);
            }

            stack.Stack(UnmaskedConnectionString, SelectionStart, SelectionLength);

            lastKeyPressedBeforeTextChange = Keys.None;
        }

        private void OnKeyPressedInternal(object sender, KeyPressEventArgs e)
        {
            // TODO Handle all shortcuts
            if (lastKeyPressedBeforeTextChange == (Keys.Control | Keys.V)
                || lastKeyPressedBeforeTextChange == (Keys.Control | Keys.C)
                || lastKeyPressedBeforeTextChange == (Keys.Control | Keys.Z)
                || lastKeyPressedBeforeTextChange == (Keys.Control | Keys.Y)
                || lastKeyPressedBeforeTextChange == Keys.Back
                || lastKeyPressedBeforeTextChange == Keys.Delete)
            {
                return;
            }

            ApplyTextChangeToUnmaskedString(new TextChange()
            {
                Beginning = SelectionStart,
                End = SelectionStart + SelectionLength,
                Modification = e.KeyChar.ToString()
            });
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            lastKeyPressedBeforeTextChange = keyData;

            if (keyData == (Keys.Control | Keys.V))
            {
                ApplyTextChangeToUnmaskedString(new TextChange()
                {
                    Beginning = SelectionStart,
                    End = SelectionStart + SelectionLength,
                    Modification = Clipboard.GetText()
                });
            }
            if (keyData == (Keys.Control | Keys.Z))
            {
                ApplyUndo();
            }
            if (keyData == (Keys.Control | Keys.Y))
            {
                ApplyRedo();
            }
            else if (keyData == Keys.Back)
            {
                if (SelectionLength == 0 && SelectionStart > 0)
                {
                    ApplyTextChangeToUnmaskedString(new TextChange()
                    {
                        Beginning = SelectionStart - 1,
                        End = SelectionStart,
                        Modification = string.Empty
                    });
                }
                else
                {
                    ApplyTextChangeToUnmaskedString(new TextChange()
                    {
                        Beginning = SelectionStart,
                        End = SelectionStart + SelectionLength,
                        Modification = string.Empty
                    });
                }
            }
            else if (keyData == Keys.Delete)
            {
                if (SelectionStart < Text.Length)
                {
                    if (SelectionLength < 1)
                    {
                        ApplyTextChangeToUnmaskedString(new TextChange()
                        {
                            Beginning = SelectionStart,
                            End = SelectionStart + 1,
                            Modification = ""
                        });
                    }
                    else
                    {
                        ApplyTextChangeToUnmaskedString(new TextChange()
                        {
                            Beginning = SelectionStart,
                            End = SelectionStart + SelectionLength,
                            Modification = ""
                        });
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ApplyTextChangeToUnmaskedString(TextChange change)
        {
            if (change.ReplaceAll)
            {
                UnmaskedConnectionString = UnmaskedConnectionString.OverwriteSegment(change.Modification, 0, UnmaskedConnectionString.Length);
            }
            else
            {
                UnmaskedConnectionString = UnmaskedConnectionString.OverwriteSegment(change.Modification, change.Beginning, change.End - change.Beginning);
            }
        }

        private void MaskPassword()
        {
            lockTextChangedEvent = true;

            var previousSelectionStart = SelectionStart;
            var previousSelectionLength = SelectionLength;

            Text = PasswordHelper.Mask(UnmaskedConnectionString);
            
            SelectionStart = previousSelectionStart;
            SelectionLength = previousSelectionLength;

            lockTextChangedEvent = false;
        }

        private void ApplyRedo()
        {
            ApplyUndoOrRedo(stack.Redo());
        }

        private void ApplyUndo()
        {
            ApplyUndoOrRedo(stack.Undo());
        }

        private void ApplyUndoOrRedo(HistoryStackItem stackItem)
        {
            lockTextChangedEvent = true;

            UnmaskedConnectionString = stackItem.Value;

            Text = PasswordHelper.Mask(UnmaskedConnectionString);

            SelectionStart = stackItem.SelectionStart;
            SelectionLength = stackItem.SelectionLength;

            lockTextChangedEvent = false;
        }
    }
}
