using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WpfApp2.Model;
using WpfApp2.Util;

namespace WpfApp2.ViewModel
{
    public class ViewModelParts
    {
        public int index = -1;

        public List<Part> AllParts = new List<Part>();
        public ObservableCollection<Part> ObservableListParts { get; set; } = new ObservableCollection<Part>();


        public ViewModelParts()
        {
            LoadList();
        }

        public string[] ItemCbo()
        {
            string[] CboSearch = { "Código", "Descrição", "Dimensão" };
            return CboSearch;
        }

        public int ConvertToInt(string item)
        {
            int result = 0;
            if (int.TryParse(item, out result) && result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
        public decimal ConvertToDecimal(string item)
        {
            decimal result = 0;
            string item2 = item.Replace(".", ",");
            if (decimal.TryParse(item2, out result) && result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        public Part NewPart(string code, string description, string length, string width)
        {
            Part newPart = new Part();
            newPart.codePart = ConvertToInt(code);
            newPart.lengthPart = ConvertToDecimal(length);
            newPart.widthPart = ConvertToDecimal(width);
            newPart.descriptionPart = description;
            newPart.dimensionPart = $"{newPart.lengthPart} X {newPart.widthPart}";
            return newPart;
        }
        public int SavePart(string code, string description, string length, string width)
        {
            if (ConvertToInt(code) > 0 && ConvertToDecimal(length) > 0 && ConvertToDecimal(width) > 0)
            {
                Part newPart = NewPart(code, description, length, width);

                if (index == -1)
                {
                    AllParts.Add(newPart);
                }
                if (index > -1)
                {
                    AllParts.RemoveAt(index);
                    AllParts.Insert(index, newPart);

                }
                Helper.Serialize(AllParts);
                RefreshDataGrid();
                index = -1;
                return 0;
            }
            else
            {
                return MessageError(code, length, width);
            }
        }
        private int MessageError(string code, string length, string width)
        {
            if (ConvertToInt(code) <= 0)
            {
                return 1;
            }
            if (ConvertToDecimal(width) <= 0)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        public void LoadList()
        {
            if (Helper.Deserialize() != null)
            {
                AllParts = Helper.Deserialize();

                foreach (Part part in AllParts)
                {
                    ObservableListParts.Add(part);
                }
            }
        }

        public void RefreshDataGrid()
        {
            ObservableListParts.Clear();
            foreach (Part part in AllParts)
            {
                ObservableListParts.Add(part);
            }
        }

        public void EditPiece(Part peca)
        {
            index = GetIndex(peca);
        }

        private int GetIndex(Part peca)
        {
            int index = -1;
            if (peca != null)
            {
                index = AllParts.IndexOf(peca);
            }
            return index;
        }

        public void DeletePart(Part part)
        {
            AllParts.RemoveAt(GetIndex(part));
            Helper.Serialize(AllParts.ToList());
            RefreshDataGrid();
        }

        private IEnumerable<Part> GetCode(List<Part> list, string search)
        {
            IEnumerable<Part> result = from item in list
                                       where item.codePart == ConvertToInt(search)
                                       select item;
            return result;
        }
        private IEnumerable<Part> GetDescription(List<Part> list, string search)
        {
            IEnumerable<Part> result = from item in list
                                       where item.descriptionPart.Contains(search)
                                       select item;
            return result;
        }
        private IEnumerable<Part> GetDimension(List<Part> list, string search)
        {
            IEnumerable<Part> result = from item in list
                                       where item.lengthPart == ConvertToDecimal(search) || item.widthPart == ConvertToDecimal(search)
                                       select item;
            return result;
        }

        public void SearchPart(int Index, string search)
        {
            if (search != "")
            {
                switch (Index)
                {
                    case 0:
                        RefreshDataGridSearch(GetCode(AllParts, search));
                        break;
                    case 1:
                        RefreshDataGridSearch(GetDescription(AllParts, search));
                        break;
                    case 2:
                        RefreshDataGridSearch(GetDimension(AllParts, search));
                        break;
                }
            }
            else 
            {
                RefreshDataGrid();
            }
        }

        public void RefreshDataGridSearch(IEnumerable<Part> AllParts)
        {
            ObservableListParts.Clear();
            foreach (Part part in AllParts)
            {
                ObservableListParts.Add(part);
            }
        }
    }
}
