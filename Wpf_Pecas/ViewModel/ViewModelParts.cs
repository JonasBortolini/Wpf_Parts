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
        public ObservableCollection<Part> ObservableListParts { get; set; } = new ObservableCollection<Part>();
        public int IndexPart = -1;
        public List<Part> AllParts = new List<Part>();
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
            item = item.Replace(".", ",");
            if (decimal.TryParse(item, out result) && result > 0)
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
            newPart.CodePart = ConvertToInt(code);
            newPart.LengthPart = ConvertToDecimal(length);
            newPart.WidthPart = ConvertToDecimal(width);
            newPart.DescriptionPart = description;
            newPart.DimensionPart = $"{newPart.LengthPart} X {newPart.WidthPart}";
            return newPart;
        }
        public int SavePart(string code, string description, string length, string width)
        {
            if (ConvertToInt(code) > 0 && ConvertToDecimal(length) > 0 && ConvertToDecimal(width) > 0)
            {
                Part newPart = NewPart(code, description, length, width);

                if (IndexPart == -1)
                {
                    AllParts.Add(newPart);
                }
                if (IndexPart > -1)
                {
                    AllParts.RemoveAt(IndexPart);
                    AllParts.Insert(IndexPart, newPart);
                }
                Helper.Serialize(AllParts);
                RefreshDataGrid();
                IndexPart = -1;
                return 0;
            }
            else
            {
                return MessageError(code, width);
            }
        }
        private int MessageError(string code, string width)
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
                RefreshDataGrid();
            }
        }
        public void RefreshDataGrid()
        {
            AllParts = AllParts.OrderBy(x => x.CodePart).ToList();
            ObservableListParts.Clear();
            foreach (Part part in AllParts)
            {
                ObservableListParts.Add(part);
            }
        }
        public void EditPiece(Part part)
        {
            IndexPart = GetIndex(part);
        }
        private int GetIndex(Part part)
        {
            int index = -1;
            if (part != null)
            {
                index = AllParts.IndexOf(part);
            }
            return index;
        }
        public void DeletePart(Part part)
        {
            AllParts.RemoveAt(GetIndex(part));
            Helper.Serialize(AllParts);
            RefreshDataGrid();
        }
        private IEnumerable<Part> GetCode(List<Part> listPart, string search)
        {
            IEnumerable<Part> result = from part in listPart
                                       where part.CodePart == ConvertToInt(search)
                                       select part;
            return result;
        }
        private IEnumerable<Part> GetDescription(List<Part> listPart, string search)
        {
            IEnumerable<Part> result = from part in listPart
                                       where part.DescriptionPart.Contains(search)
                                       select part;
            return result;
        }
        private IEnumerable<Part> GetDimension(List<Part> listPart, string search)
        {
            IEnumerable<Part> result = from part in listPart
                                       where part.LengthPart == ConvertToDecimal(search) || part.WidthPart == ConvertToDecimal(search)
                                       select part;
            return result;
        }
        public void SearchPart(int index, string search)
        {
            if (search != "")
            {
                switch (index)
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
        public void RefreshDataGridSearch(IEnumerable<Part> allParts)
        {
            ObservableListParts.Clear();
            foreach (Part part in allParts)
            {
                ObservableListParts.Add(part);
            }
        }
    }
}
