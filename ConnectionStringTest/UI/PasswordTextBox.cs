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

        public string UnmaskedConnectionString { get; private set; }

        private bool lockTextChangedEvent = false;

        public PasswordTextBox(IPasswordHelper passwordHelper)
        {
            PasswordHelper = passwordHelper;

            UnmaskedConnectionString = string.Empty;
            Text = string.Empty;

            Handlers = new List<IEventHandler>();

            KeyPress += OnKeyPressedInternal;
            TextChanged += TextChangedInternal;
        }

        public void AddEventHandler(IEventHandler handler)
        {
            Handlers.Add(handler);
        }

        private async void TextChangedInternal(object sender, EventArgs e)
        {
            if (lockTextChangedEvent) 
            {
                return;
            };

            if (lastKeyPressedBeforeTextChange == Keys.None
                || lastKeyPressedBeforeTextChange == Keys.Down
                || lastKeyPressedBeforeTextChange == Keys.Up)
            {
                if (AutoCompleteCustomSource.Contains(Text))
                {
                    ApplyTextChangeToUnmaskedString(new TextChange()
                    {
                        Beginning = 0,
                        End = -1,
                        Modification = Text
                    });
                }
            }

            MaskPassword();

            foreach (var handler in Handlers)
            {
                await handler.Handle(Event.ConnectionStringBoxTextChanged, this);
            }

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
                // TODO
            }
            if (keyData == (Keys.Control | Keys.Y))
            {
                // TODO
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

            // TODO This erases value history. It breaks the ctrl+z/ctrl+y function. Fix.
            Text = PasswordHelper.Mask(UnmaskedConnectionString);
            
            SelectionStart = previousSelectionStart;
            SelectionLength = previousSelectionLength;

            lockTextChangedEvent = false;
        }
    }
}
