using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2.Model;
using WpfApp2.ViewModel;

namespace Wpf_Pecas.View
{
    public partial class ViewParts : UserControl
    {
        private ViewModelParts viewModelParts => (ViewModelParts)this.DataContext;
        public ViewParts()
        {
            InitializeComponent();
            cboSearch.ItemsSource = viewModelParts.ItemCbo();
            DisableButtons();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch (viewModelParts.SavePart(txbCode.Text, txbDescription.Text, txbLength.Text, txbWidth.Text))
            {
                case 0:
                    MessageBox.Show("Salvo com sucesso", "Salvar", MessageBoxButton.OK);
                    ClearTextBox();
                    DisableButtons();
                    break;
                case 1:
                    MessageBox.Show("Codigo invalido digite novamente", "Error", MessageBoxButton.OK);
                    txbCode.Text = "";
                    break;
                case 2:
                    MessageBox.Show("Largura invalida digite novamente", "Error", MessageBoxButton.OK);
                    txbWidth.Text = "";
                    break;
                case 3:
                    MessageBox.Show("Comprimento invalido digite novamente", "Error", MessageBoxButton.OK);
                    txbLength.Text = "";
                    break;
            }
        }
        private void DisableButtons()
        {
            btnCancel.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Part part = (Part)dataGridParts.SelectedItem;
            viewModelParts.EditPiece(part);
            txbCode.Text = part.codePart.ToString();
            txbDescription.Text = part.descriptionPart;
            txbLength.Text = part.lengthPart.ToString();
            txbWidth.Text = part.widthPart.ToString();
            btnCancel.IsEnabled = true;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModelParts.DeletePart((Part)dataGridParts.SelectedItem);
                ClearTextBox();
                DisableButtons();
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            viewModelParts.SearchPart(cboSearch.SelectedIndex, txbSearch.Text);
        }
        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            viewModelParts.RefreshDataGrid();
            ClearTextBox();
        }
        private void ClearTextBox()
        {
            txbCode.Text = "";
            txbDescription.Text = "";
            txbLength.Text = "";
            txbWidth.Text = "";
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            btnCancel.IsEnabled = false;
            viewModelParts.EditPiece(null);
        }

        private void dataGridParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }
    }
}
