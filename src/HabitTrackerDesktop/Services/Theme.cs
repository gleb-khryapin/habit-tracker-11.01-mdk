using System.Runtime.InteropServices;

namespace HabitTrackerDesktop.Services;

public static class Theme
{
    public static readonly Color Background = Color.FromArgb(13, 10, 20);
    public static readonly Color Panel = Color.FromArgb(21, 17, 31);
    public static readonly Color Input = Color.FromArgb(29, 23, 41);
    public static readonly Color Border = Color.FromArgb(58, 47, 86);
    public static readonly Color Selection = Color.FromArgb(48, 36, 84);
    public static readonly Color Text = Color.FromArgb(236, 232, 246);
    public static readonly Color Muted = Color.FromArgb(150, 141, 175);
    public static readonly Color Accent = Color.FromArgb(139, 92, 246);
    public static readonly Color Success = Color.FromArgb(125, 211, 176);
    public static readonly Color Danger = Color.FromArgb(242, 133, 138);

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr window, int attribute, ref int value, int size);

    public static void Apply(Form form)
    {
        form.BackColor = Background;
        form.ForeColor = Text;

        int darkTitleBar = 1;
        DwmSetWindowAttribute(form.Handle, 20, ref darkTitleBar, sizeof(int));

        StyleControls(form.Controls);
    }

    private static void StyleControls(Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            string tag = control.Tag as string;

            if (control is Button)
            {
                StyleButton((Button)control, tag);
            }
            else if (control is DataGridView)
            {
                StyleGrid((DataGridView)control);
            }
            else if (control is TextBox)
            {
                TextBox textBox = (TextBox)control;
                textBox.BackColor = Input;
                textBox.ForeColor = Text;
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox)
            {
                ComboBox comboBox = (ComboBox)control;
                comboBox.BackColor = Input;
                comboBox.ForeColor = Text;
                comboBox.FlatStyle = FlatStyle.Flat;
            }
            else if (control is NumericUpDown)
            {
                control.BackColor = Input;
                control.ForeColor = Text;
            }
            else if (control is Label)
            {
                if (tag == "muted")
                {
                    control.ForeColor = Muted;
                }
                if (tag == "accent")
                {
                    control.ForeColor = Accent;
                }
            }
            else if (control is Panel)
            {
                StyleControls(control.Controls);
            }
        }
    }

    private static void StyleButton(Button button, string tag)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.Cursor = Cursors.Hand;
        button.BackColor = Input;
        button.ForeColor = Text;
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.BorderSize = 1;

        if (tag == "primary")
        {
            button.BackColor = Accent;
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderSize = 0;
        }
        if (tag == "danger")
        {
            button.ForeColor = Danger;
        }
    }

    private static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Panel;
        grid.GridColor = Input;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        grid.ColumnHeadersDefaultCellStyle.BackColor = Input;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Muted;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Input;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Muted;

        grid.DefaultCellStyle.BackColor = Panel;
        grid.DefaultCellStyle.ForeColor = Text;
        grid.DefaultCellStyle.SelectionBackColor = Selection;
        grid.DefaultCellStyle.SelectionForeColor = Text;
    }
}
