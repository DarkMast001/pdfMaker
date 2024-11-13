using pdfMaker.Model;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace pdfMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BindingList<Person> _personList;

        public MainWindow()
        {
            InitializeComponent();

            _personList = new BindingList<Person>()
            {
                new Person { Number = 1, LastName = "Иванов", FirstName = "Иван" },
                new Person { Number = 2, LastName = "Сидоров", FirstName = "Сидр" },
                new Person { Number = 3, LastName = "Петров", FirstName = "Петр" }
            };

            personsDataGrid.ItemsSource = _personList;
        }

        private void textBoxEnterHeader_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (labelHeader != null)
            {
                labelHeader.Content = textBoxEnterHeader.Text;
            }
        }

        private void textBoxEnterHeader_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxEnterHeader.Text == "Заголовок документа")
            {
                textBoxEnterHeader.Text = "";
            }
        }

        private void textBoxEnterHeader_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxEnterHeader.Text == "")
            {
                textBoxEnterHeader.Text = "Заголовок документа";
            }
        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            // Инициализируем документ
            Document doc = new Document();
            // Добавляем страницу
            Aspose.Pdf.Page page = doc.Pages.Add();

            // Делам заголовок
            TextFragment header = new TextFragment(labelHeader.Content.ToString() + "\n");
            header.TextState.FontSize = 16;
            header.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center;
            page.Paragraphs.Add(header);

            // Делаем таблицу
            Aspose.Pdf.Table table = new Aspose.Pdf.Table()
            {
                ColumnWidths = "50 200 200",
                Border = new BorderInfo(BorderSide.Box, 1f, Aspose.Pdf.Color.DarkSlateGray),
                DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Aspose.Pdf.Color.Black),
                DefaultCellPadding = new MarginInfo(4.5, 4.5, 4.5, 4.5),
                Margin =
                {
                    Bottom = 10
                },
                DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("TimesNewRoman")
                },
                HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center
            };

            Aspose.Pdf.Row headerRow = table.Rows.Add();
            headerRow.Cells.Add("Номер");
            headerRow.Cells.Add("Фамилия");
            headerRow.Cells.Add("Имя");
            foreach (Cell headerRowCell in headerRow.Cells)
            {
                headerRowCell.BackgroundColor = Aspose.Pdf.Color.Gray;
                headerRowCell.DefaultCellTextState.ForegroundColor = Aspose.Pdf.Color.Black;
                headerRowCell.DefaultCellTextState.FontStyle = Aspose.Pdf.Text.FontStyles.Bold;
            }
            for (int row_count = 0; row_count < _personList.Count; row_count++)
            {
                Aspose.Pdf.Row dataRow = table.Rows.Add();
                dataRow.Cells.Add(_personList[row_count].Number.ToString());
                dataRow.Cells.Add(_personList[row_count].LastName);
                dataRow.Cells.Add(_personList[row_count].FirstName);
            }

            page.Paragraphs.Add(table);

            doc.Save("document.pdf");
        }
    }
}