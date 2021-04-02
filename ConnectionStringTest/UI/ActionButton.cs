using ConnectionStringTest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public class ActionButton : Button
    {
        private Action currentAction;
        public Action CurrentAction
        {
            get
            {
                return currentAction;
            }
            set 
            {
                currentAction = value;

                if (value == Action.FireTest)
                {
                    BackgroundImage = Properties.Resources.firetest;
                }
                else if(value == Action.Cancel)
                {
                    BackgroundImage = Properties.Resources.canceltest;
                }
                else
                {
                    throw new UnhandledEnumException(value);
                }
            }
        }

        public ActionButton()
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            CausesValidation = false;
            FlatAppearance.BorderSize = 0;
            Location = new System.Drawing.Point(757, 2);
            Margin = new Padding(0);
            Name = "actionButton";
            Size = new System.Drawing.Size(22, 22);
            TabIndex = 6;
            UseVisualStyleBackColor = true;

            CurrentAction = Action.FireTest;
        }

        public enum Action
        {
            FireTest,
            Cancel
        }
    }
}
