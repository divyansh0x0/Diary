using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace LifeInDiary.Libs.Components
{
    class TextEditing
    {
        private RichTextBox TextEditor;
        public TextEditing(RichTextBox RichTextBox)
        {
            this.TextEditor = RichTextBox;
        }

        public void RemoveAllKeyBindings()
        {
            TextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.U, ModifierKeys.Control));
            TextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.B, ModifierKeys.Control));
            TextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.I, ModifierKeys.Control));
            TextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.S, ModifierKeys.Control));
            //TextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.A, ModifierKeys.Control));
            TextEditor.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.Enter, ModifierKeys.None));
        }
        /// <summary>
        /// Toggles the bold text in the editor
        /// </summary>
        public void toggleBold()
        {
            EditingCommands.ToggleBold.Execute(null, TextEditor);
        }
        /// <summary>
        /// Toggles the italic text in the editor
        /// </summary>
        public void toggleItalic()
        {
            EditingCommands.ToggleItalic.Execute(null, TextEditor);
        }
        /// <summary>
        /// Toggles the underlined text in the editor
        /// </summary>
        public void toggleUnderline()
        {
            EditingCommands.ToggleUnderline.Execute(null, TextEditor);
        }

    }
}
