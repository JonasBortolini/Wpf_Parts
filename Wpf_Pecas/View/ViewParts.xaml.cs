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
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch (viewModelParts.SavePart(txbCode.Text, txbDescription.Text, txbLength.Text, txbWidth.Text))
            {
                case 0:
                    MessageBox.Show("Salvo com sucesso", "Salvar", MessageBoxButton.OK);
                    ClearTextBox();
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
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Part part = (Part)dataGridParts.SelectedItem;
            viewModelParts.EditPiece(part);
            if (part != null)
            {
                txbCode.Text = part.codePart.ToString();
                txbDescription.Text = part.descriptionPart;
                txbLength.Text = part.lengthPart.ToString();
                txbWidth.Text = part.widthPart.ToString();
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridParts.SelectedItem != null)
            {
                if (MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    viewModelParts.DeletePart((Part)dataGridParts.SelectedItem);
                    ClearTextBox();
                }
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

    }
}
