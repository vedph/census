using Census.Core;
using Census.MySql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace CensusSqlDemo
{
    public partial class MainWindow : Window
    {
        private readonly ISqlQueryBuilder _sqlBuilder;

        public MainWindow()
        {
            InitializeComponent();
            _sqlBuilder = new MySqlQueryBuilder(new StandardTextFilter());
        }

        private static List<int> ParseIds(string text)
        {
            return (from token in text.Split(new[] { ' ', '\t' },
                StringSplitOptions.RemoveEmptyEntries)
                   select int.Parse(token, CultureInfo.InvariantCulture)).ToList();
        }

        private ActFilter GetActFilter()
        {
            return new ActFilter
            {
                ArchiveId = int.Parse(_txtArchiveId.Text, CultureInfo.InvariantCulture),
                BookId = int.Parse(_txtBookId.Text, CultureInfo.InvariantCulture),
                BookYearMin = short.Parse(_txtBookYearMin.Text, CultureInfo.InvariantCulture),
                BookYearMax = short.Parse(_txtBookYearMax.Text, CultureInfo.InvariantCulture),
                Description = _txtDescription.Text,
                ActTypeId = int.Parse(_txtActTypeId.Text, CultureInfo.InvariantCulture),
                FamilyId = int.Parse(_txtFamilyId.Text, CultureInfo.InvariantCulture),
                CompanyId = int.Parse(_txtCompanyId.Text, CultureInfo.InvariantCulture),
                PlaceId = int.Parse(_txtPlaceId.Text, CultureInfo.InvariantCulture),
                Label = _txtLabel.Text,
                CategoryIds = ParseIds(_txtCategoryIds.Text),
                ProfessionIds = ParseIds(_txtProfessionIds.Text),
                PartnerIds = ParseIds(_txtPartnerIds.Text)
            };
        }

        private void OnGenerateSearchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var t = _sqlBuilder.BuildGetActs(GetActFilter());
                _txtSql.Text = t.Item1 + "\r\n\r\n" + t.Item2;
                _tabs.SelectedItem = _tabSql;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK);
            }
        }

        private void OnGenerateLookupClick(object sender, RoutedEventArgs e)
        {
            if (_cboTables.SelectedIndex == -1) return;
            try
            {
                _txtSql.Text = _sqlBuilder.BuildLookup(
                    (DataEntityType)_cboTables.SelectedIndex,
                    _txtLookupFilter.Text,
                    int.Parse(_txtLookupLimit.Text, CultureInfo.InvariantCulture));
                _tabs.SelectedItem = _tabSql;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message, Title, MessageBoxButton.OK);
            }
        }
    }
}
