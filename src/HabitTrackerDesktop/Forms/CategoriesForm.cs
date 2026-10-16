using HabitTrackerDesktop.Data;
using HabitTrackerDesktop.Models;
using HabitTrackerDesktop.Services;

namespace HabitTrackerDesktop.Forms;

public partial class CategoriesForm : Form
{
    public CategoriesForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        LoadCategories();
    }

    private void LoadCategories()
    {
        grid.Rows.Clear();

        foreach (Category category in CategoryRepository.GetAll())
        {
            int index = grid.Rows.Add(category.Name);
            grid.Rows[index].Tag = category;
        }

        btnDelete.Enabled = grid.Rows.Count > 0;
    }

    private Category GetSelectedCategory()
    {
        if (grid.SelectedRows.Count == 0)
        {
            return null;
        }
        return grid.SelectedRows[0].Tag as Category;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string name = txtName.Text.Trim();

        string error = Rules.CheckCategoryName(name);
        if (error == null && NameExists(name))
        {
            error = "Такая категория уже есть";
        }
        if (error != null)
        {
            Dialogs.Error(error);
            return;
        }

        Category category = new Category();
        category.Name = name;
        CategoryRepository.Add(category);

        txtName.Clear();
        LoadCategories();
    }

    private bool NameExists(string name)
    {
        foreach (Category category in CategoryRepository.GetAll())
        {
            if (string.Equals(category.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        Category category = GetSelectedCategory();
        if (category == null)
        {
            return;
        }

        if (Dialogs.Confirm("Удалить категорию «" + category.Name + "»? Привычки из неё останутся, но будут без категории."))
        {
            CategoryRepository.Delete(category.Id);
            LoadCategories();
        }
    }
}
