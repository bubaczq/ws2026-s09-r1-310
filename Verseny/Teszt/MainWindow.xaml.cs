using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Diagnostics;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Verseny
{
    public partial class MainWindow : Window
    {

        private List<Customer> LoadedFile = new List<Customer>();
        private List<Customer> CustomersList = new List<Customer>();
        private List<Error> errors = new List<Error>();
        private List<Customer> SearchList = new List<Customer>();
        private static readonly HttpClient client = new HttpClient();
        private int page = 0;
        private int Loadedpage = 0;
        private int Errorpage = 0;
        public MainWindow()
        {
            InitializeComponent();
            LoadCustomersFromApi();

            ErrorpageMinus_btn.Visibility = Visibility.Hidden;
            LoadedpageMinus_btn.Visibility = Visibility.Hidden;
            pageMinus_btn.Visibility = Visibility.Hidden;
            errors_datagrid.ItemsSource = errors;
            progress_lb.Visibility = Visibility.Hidden;
        }

        private async Task LoadCustomersFromApi()
        {
            try
            {
                var response = await client.GetStringAsync("http://127.0.0.1:3000/api/customers/");
                var jsonResponse = JsonDocument.Parse(response);

                foreach (var customer in jsonResponse.RootElement.EnumerateArray())
                {
                    string csvLine = $"{customer.GetProperty("id").ToString()}," +
                             $"{customer.GetProperty("firstName").ToString()}," +
                             $"{customer.GetProperty("lastName").ToString()}," +
                             $"{customer.GetProperty("age").ToString()}," +
                             $"{customer.GetProperty("gender").ToString()}," +
                             $"{customer.GetProperty("postalCode").ToString()}," +
                             $"{customer.GetProperty("email").ToString()}," +
                             $"{customer.GetProperty("phone").ToString()}," +
                             $"{customer.GetProperty("membership").ToString()}," +
                             $"{customer.GetProperty("joinedAt").ToString()}," +
                             $"{customer.GetProperty("lastPurchaseAt").ToString()}," +
                             $"{customer.GetProperty("totalSpending").ToString()}," +
                             $"{customer.GetProperty("averageOrderValue").ToString()}," +
                             $"{customer.GetProperty("frequency").ToString()}," +
                             $"{customer.GetProperty("preferredCategory").ToString()}," +
                             $"{customer.GetProperty("churned").ToString()}";

                    CustomersList.Add(new Customer(csvLine));
                }
                
                OverviewMetrics();
                GenderDistribution();
                MembersipLevelsDistribution();
                MostPurchasedCategories();
                TopSpenders();
                NewCustomersOverTime();
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a vásárlók betöltése során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OverviewMetrics()
        {
            //Össes felhasználó
            totalCustomers_lb.Content = $"Total Customers: {CustomersList.Count}";

            //Átlag életkor
            int index = 0;
            double totalAge = 0.0;
            foreach (var customer in CustomersList)
            {
                if (double.TryParse(customer.age, out double age))
                {
                    index++;
                    totalAge += age;
                }
            }
            double averageAge = totalAge / index;
            averageAge_lb.Content = "Average Age: " + averageAge.ToString();

            //Leg Preferáltabb kategória
            string mostCommonCategory = CustomersList
                .GroupBy(c => c.preferredCategory)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
            mostFrequentPurchaseCategory_lb.Content = "Most Frequent Purchase Category: " + mostCommonCategory;

            //Átlag vásárlási összeg
            index = 0;
            double totalOrderValue = 0.0;
            foreach (var customer in CustomersList)
            {
                if (double.TryParse(customer.averageOrderValue, out double average))
                {
                    index++;
                    totalOrderValue += average;
                }
            }
            double totalOrderValueAverage = totalOrderValue / index;
            averageOrderValue_lb.Content = "Average Order Value: " + totalOrderValueAverage.ToString("F2");

            //Évenkénti Frequency
            var yearlyFrequency = CustomersList
                    .Where(c => !string.IsNullOrWhiteSpace(c.joinedAt) &&
                                 !string.IsNullOrWhiteSpace(c.frequency))
                    .GroupBy(c => c.joinedAt.Split('-')[0]) // Csoportosítás év szerint
                    .Select(g => new // Új osztályt hozunk létre
                    {
                        Year = g.Key,
                        TotalFrequency = g.Sum(c =>
                        {
                            double frequencyValue;
                            return double.TryParse(c.frequency.Replace(".", ","), out frequencyValue) ? frequencyValue : 0.0;
                        })
                    })
                    .Where(x => x.TotalFrequency > 0) // Csak a pozitív frekvenciák
                    .OrderByDescending(x => x.Year) // Csökkenő sorrend az év szerint
                    .ToList();

            // DataGrid-be töltés
            frequencyDataGrid.ItemsSource = yearlyFrequency;

            //Összesen menyit költöttek
            totalPurchaseValue_lb.Content = "Total Purchase Value: " +
                CustomersList.Sum(x =>
                {
                    if (double.TryParse(x.totalSpending.Replace(".", ","), out double totalSpending))
                    {
                        return totalSpending; // Ha sikeres a konverzió, visszaadjuk az értéket
                    }
                    return 0; // Ha nem sikerül, 0-t adunk vissza
                }).ToString("F2");
        }

        private void GenderDistribution()
        {
            int totalCustomersGender = CustomersList.Where(x => x.gender != "").Count();
            var genders = CustomersList
                .Where(x => x.gender != "" && (x.gender != "F" || x.gender != "M"))
                .GroupBy(c => c.gender)
                .Select(g => new
                {
                    Gender = g.Key,
                    Percentage = Math.Round((double)g.Count() / totalCustomersGender * 100, 0)
                });
            SeriesCollection gendersSeries = new SeriesCollection();
            foreach (var gender in genders)
            {
                if (gender.Percentage != 0)
                {
                    gendersSeries.Add(new PieSeries
                    {
                        Title = gender.Gender == "F" ? "Fiú" : "Lány",
                        Values = new ChartValues<double> { gender.Percentage },
                        DataLabels = true
                    });
                }

            }
            Nemek.Series = gendersSeries;
        }

        private void MembersipLevelsDistribution()
        {
            int totalCustomersMembership = CustomersList.Where(x => x.membership != "").Count();
            var membershipPercentage = CustomersList
                .GroupBy(c => c.membership)
                .Select(g => new
                {
                    MembershipType = g.Key,
                    Percentage = Math.Round((double)g.Count() / totalCustomersMembership * 100, 0)
                });
            SeriesCollection membershipsSeries = new SeriesCollection();
            foreach (var membership in membershipPercentage)
            {
                membershipsSeries.Add(new PieSeries
                {
                    Title = membership.MembershipType,
                    Values = new ChartValues<double> { membership.Percentage },
                    DataLabels = true
                });
            }
            Tagsag.Series = membershipsSeries;
        }

        private void MostPurchasedCategories()
        {
            SeriesCollection MostPurchaseCategories = new SeriesCollection();
            var MostPurchaseCategoriesList = CustomersList
                .Where(x => !string.IsNullOrEmpty(x.preferredCategory))
                .GroupBy(x => x.preferredCategory)
                .Select(x => new { Category = x.Key, CategoryCount = x.Count() })
                .ToList();

            // Régi adatok törlése
            MostPurchaseCategories.Clear();

            foreach (var m in MostPurchaseCategoriesList)
            {
                MostPurchaseCategories.Add(new ColumnSeries
                {
                    Title = m.Category,
                    Values = new ChartValues<int> { m.CategoryCount },
                    DataLabels = true
                });
            }
            MostPurchasedCategory.Series = MostPurchaseCategories;
        }

        private void TopSpenders()
        {
            Top10DataGrid.ItemsSource = null;

            var Top10Spender = CustomersList
                .GroupBy(x => x.firstName + " " + x.lastName)
                .Select(g => new
                {
                    Name = g.Key,
                    Spending = g.Sum(x => decimal.Parse(x.totalSpending.Replace(".", ",")))
                })
                .OrderByDescending(x => x.Spending)
                .Take(10)
                .Select((spender, index) => new
                {
                    Rank = index + 1,
                    Name = spender.Name,
                    Spending = spender.Spending
                })
                .ToList();

            Top10DataGrid.ItemsSource = Top10Spender;

        }

        private void NewCustomersOverTime()
        {
            // Adatok előkészítése
            SeriesCollection YearlyTrend = new SeriesCollection();
            var YearlyTrends = CustomersList
                .Where(x => !string.IsNullOrEmpty(x.joinedAt))
                .GroupBy(x => x.joinedAt.Split('-')[0]) // Csoportosítás év szerint
                .OrderBy(x => x.Key)
                .Select(x => new
                {
                    Year = x.Key,
                    Customers = x.Count()
                })
                .ToList();

            // X tengely címkék (évek)
            var YearLabels = YearlyTrends.Select(x => x.Year).ToList();

            // Adatsor létrehozása
            var lineSeries = new LineSeries
            {
                Title = "New Customers Over Time",
                Values = new ChartValues<ObservablePoint>(),
                DataLabels = true
            };

            //Adatok értékeinek feltöltése évenként
            foreach (var year in YearlyTrends)
            {
                lineSeries.Values.Add(new ObservablePoint(int.Parse(year.Year), year.Customers)); 
            }
                
            foreach(var year in YearLabels)
            {
                Debug.WriteLine(year);
            }

            YearlyTrend.Add(lineSeries);

            //Adatok beállitások
            YearlyTrendChart.Series = YearlyTrend;

            YearlyTrendChart.AxisX[0].Title = "Years";
            YearlyTrendChart.AxisX[0].Labels = YearLabels;
            YearlyTrendChart.AxisX[0].Separator.Step = 1;
            YearlyTrendChart.AxisX[0].LabelsRotation = 45;
        }

        private async Task LoadCustomersFromFile(string filePath)
        {
            try
            {
                //listák törlése 
                LoadedFile.Clear();
                errors.Clear();

                //Ellenőrzés hogy a ffile elég hosszú-e
                var lines = File.ReadAllLines(filePath);
                if (lines.Length < 2)
                {
                    MessageBox.Show("A fájl nem tartalmaz elegendő sort!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //Lista feltöltése az ellenőrzések után
                int lineNumber = 1;
                foreach (var line in lines.Skip(1))
                {
                    lineNumber++;
                    //A megfelelő adathossz ellenőrzése
                    var values = line.Split(',');
                    if (values.Length < 16)
                    {
                        errors.Add(new Error(lineNumber, ",Túl kevés adat!"));
                        continue;
                    }
                    if (values.Length > 16)
                    {
                        errors.Add(new Error(lineNumber, ",Veszélyes karakter észlelve!"));
                        continue;
                    }

                    try
                    {
                        //Id Létezésének ellen őrzése
                        string id = values[0]?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(id))
                        {
                            errors.Add(new Error(lineNumber, ",Missing ID."));
                            continue;
                        }

                        //Életkór ellenőrzése
                        if (!float.TryParse(values[3].Replace(".", ","), out float age) && values[3] != "")
                        {
                            errors.Add(new Error(lineNumber, ",Invalid age."));
                            values[3] = "";
                        }

                        //Email Ellenőrzése
                        if (!values[6].Contains("@") || !values[6].Contains("."))
                            errors.Add(new Error(lineNumber, ",Invalid email format."));

                        //Nemek ellenőrzése
                        if (values[4] != "F" && values[4] != "M" && values[4] != "")
                            errors.Add(new Error(lineNumber, ",Invalid gender."));

                        //Telefonszám ellenőrzése
                        if ((!long.TryParse(values[7], out long number) || values[7].Length < 10) && !string.IsNullOrWhiteSpace(values[7]))
                        {
                            errors.Add(new Error(lineNumber, ",Invalid phone number."));
                            values[7] = "";
                        }
                        else values[7] = values[7].Replace("+", "");
                        
                        //Tagság ellenőrzése
                        string membership = values[8].ToLower().Trim();
                        switch (membership)
                        {
                            case "basic":
                            case "bronze":
                                values[8] = "bronze";
                                break;
                            case "silver":
                            case "gold":
                                values[8] = membership;
                                break;
                            default:
                                Debug.WriteLine(values[8]);
                                values[8] = "";
                                errors.Add(new Error(lineNumber, ",Invalid membership status."));
                                break;
                        }

                        //Dátum
                        string dateFormat = "yyyy-MM-dd";
                        DateTime joinDate, lastPurchaseDate;

                        //Csatlakozási dátum ellenőrzése
                        if (!DateTime.TryParseExact(values[9], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out joinDate))
                        {
                            errors.Add(new Error(lineNumber, ",Invalid joining date."));
                            values[9] = "";
                        }
                        else if (joinDate.Year > 2025 || joinDate.Year < 2000)
                        {
                            errors.Add(new Error(lineNumber, ",Invalid joining date year."));
                            values[9] = "";
                        }

                        //Utolsó vásárlási dátum ellenőrzése
                        if (!DateTime.TryParseExact(values[10], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out lastPurchaseDate))
                        {
                            errors.Add(new Error(lineNumber, ",Invalid last purchase date."));
                            values[10] = "";
                        }
                        else if (lastPurchaseDate.Year > 2025 || lastPurchaseDate.Year < 2000)
                        {
                            errors.Add(new Error(lineNumber, ",Invalid last purchase date year."));
                            values[10] = "";
                        }

                        //a csatlakozási és vásárlási Dátum ellenőrzése
                        if (!string.IsNullOrWhiteSpace(values[9]) && !string.IsNullOrWhiteSpace(values[10]))
                        {
                            string[] joinDateParts = values[9].Split('-');
                            string[] lastPurchaseDateParts = values[10].Split('-');

                            if (joinDateParts.Length == 3 && lastPurchaseDateParts.Length == 3 &&
                                int.TryParse(joinDateParts[0], out int joinYear) &&
                                int.TryParse(joinDateParts[1], out int joinMonth) &&
                                int.TryParse(joinDateParts[2], out int joinDay) &&
                                int.TryParse(lastPurchaseDateParts[0], out int lastYear) &&
                                int.TryParse(lastPurchaseDateParts[1], out int lastMonth) &&
                                int.TryParse(lastPurchaseDateParts[2], out int lastDay))
                            {
                                joinDate = new DateTime(joinYear, joinMonth, joinDay);
                                lastPurchaseDate = new DateTime(lastYear, lastMonth, lastDay);

                                if (lastPurchaseDate < joinDate)
                                {
                                    errors.Add(new Error(lineNumber, ",Last purchase date earlier than join date."));
                                    values[10] = "";
                                }
                            }
                        }

                        //Preferált kategóriák ellenőrzése
                        string[] invalidCategories = { "Unknown", "TBD", "To Be Determined", "N/A" };
                        if (invalidCategories.Contains(values[14]))
                        {
                            errors.Add(new Error(lineNumber, ",Invalid preferred category."));
                            values[14] = "";
                        }

                        //Churned érték ellenőrzése
                        if (!string.IsNullOrWhiteSpace(values[15]))
                        {
                            string churnedValue = values[15].Trim().ToLower();

                            if (churnedValue == "true" || churnedValue == "yes" || churnedValue == "1")
                            {
                                values[15] = "true";
                            }
                            else if (churnedValue == "false" || churnedValue == "no" || churnedValue == "0")
                            {
                                values[15] = "false";
                            }
                            else
                            {
                                errors.Add(new Error(lineNumber, ",Invalid churned value."));
                                values[15] = "";
                            }
                        }

                        //A Customer osztály példányosítása a CSV-ből
                        var customer = new Customer(string.Join(",", values));

                        // Az ügyfél hozzáadása a LoadedFile (Customers) listához
                        LoadedFile.Add(customer); // Csak sikeres feldolgozás esetén adja hozzá a listához
                        Load();
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new Error(lineNumber, $",Error during processing: {ex.Message}"));
                    }
                }

                MessageBox.Show($"Feldolgozás kész!\nSikeres rekordok: {LoadedFile.Count}\nHibák: {errors.Count}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a fájl feldolgozása során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task Upload()
        {
            try
            {
                progress_lb.Visibility=Visibility.Visible;
                int index = 0;
                int batchSize = 30;
                int totalCount = LoadedFile.Count;

                // A teljes lista darabokra bontása és küldése
                while (index < totalCount)
                {
                    progress_lb.Content = $"Progress: [{index}/{LoadedFile.Count}] Please do not close the program!";
                    var currentBatch = LoadedFile.Skip(index).Take(batchSize).ToList();
                    var jsonCustomers = JsonSerializer.Serialize(currentBatch);
                    var content = new StringContent(jsonCustomers, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:3000/api/customers/", content);

                    if (!response.IsSuccessStatusCode) errors.Add(new Error(index / batchSize + 1, "Hiba a Customers mentésekor az API-ba: " + response.ReasonPhrase));
                    
                    index += batchSize;
                    Debug.WriteLine(index);

                    await Task.Delay(100); 
                }

                LoadCustomersFromApi();
                progress_lb.Content = $"Progress: [{LoadedFile.Count}/{LoadedFile.Count}] Please do not close the program!";
                MessageBox.Show($"Feldolgozás kész!");
                progress_lb.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"HTTP hiba: {ex.Message}\nInnerException: {ex.InnerException?.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                progress_lb.Visibility = Visibility.Hidden;
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Fájl megnyitása",
                Filter = "CSV fájlok (*.csv)|*.csv|Összes fájl (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                LoadCustomersFromFile(filePath);
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveJSONFileDialog = new SaveFileDialog
            {
                Title = "Javított adatok JSON letöltése",
                Filter = "JSON fájlok (*.json)|*.json",
                DefaultExt = "json"
            };

            if (saveJSONFileDialog.ShowDialog() == true)
            {
                try
                {
                    string jsonData = JsonSerializer.Serialize(CustomersList);
                    File.WriteAllText(saveJSONFileDialog.FileName, jsonData);
                    MessageBox.Show("A fájl sikeresen mentve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt a mentés során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
       
        private void ErrorsDownload_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveCSVFileDialog = new SaveFileDialog
            {
                Title = "Errors CSV letöltése",
                Filter = "CSV fájlok (*.csv)|*.csv",
                DefaultExt = "csv"
            };

            if (saveCSVFileDialog.ShowDialog() == true)
            {
                try
                {
                    StringBuilder csvContent = new StringBuilder();
                    csvContent.AppendLine("Row Number,Description"); // Fejléc sor

                    foreach (var error in errors)
                    {
                        csvContent.AppendLine(error.RowNumber.ToString()+error.Description);
                    }

                    File.WriteAllText(saveCSVFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);
                    MessageBox.Show("A fájl sikeresen mentve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt a mentés során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DatabaseAdd_btn_Click(object sender, RoutedEventArgs e)
        {
            Upload();
        }

        private void Load()
        {
            if (SearchList.Count > 0) customers_datagrid.ItemsSource = SearchList.Skip(page * 25).Take(25).ToList();
            else customers_datagrid.ItemsSource=CustomersList.Skip(page*25).Take(25).ToList();
            loaded_datagrid.ItemsSource = LoadedFile.Skip(Loadedpage*25).Take(25).ToList();
            errors_datagrid.ItemsSource = errors.Skip(Errorpage * 25).Take(25).ToList();


            if (LoadedFile.Count > 0)
            {
                DatasDownload_btn.Visibility = Visibility.Visible;
                ErrorsDownload_btn.Visibility = Visibility.Visible;
                DatabaseAdd_btn.Visibility = Visibility.Visible;
                Errors_tabitem.Visibility = Visibility.Visible;
                LoadedFile_tabitem.Visibility = Visibility.Visible;
            }
            else
            {
                DatasDownload_btn.Visibility = Visibility.Hidden;
                ErrorsDownload_btn.Visibility = Visibility.Hidden;
                DatabaseAdd_btn.Visibility = Visibility.Hidden;
                Errors_tabitem.Visibility = Visibility.Hidden;
                LoadedFile_tabitem.Visibility = Visibility.Hidden;
            }
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(kereses_tb.Text.Length);

            SearchList.Clear();
            if (kereses_tb.Text.Length > 0)
                SearchList = CustomersList.Where(x => x.id.Trim().ToLower().Contains(kereses_tb.Text) || x.firstName.Trim().ToLower().Contains(kereses_tb.Text) || x.lastName.Trim().ToLower().Contains(kereses_tb.Text) || x.age.Trim().ToLower().Contains(kereses_tb.Text) || x.gender.Trim().ToLower().Contains(kereses_tb.Text) || x.postalCode.Trim().ToLower().Contains(kereses_tb.Text) || x.email.Trim().ToLower().Contains(kereses_tb.Text) || x.phone.Trim().ToLower().Contains(kereses_tb.Text) || x.membership.Trim().ToLower().Contains(kereses_tb.Text) || x.joinedAt.Trim().ToLower().Contains(kereses_tb.Text) || x.lastPurchaseAt.Trim().ToLower().Contains(kereses_tb.Text) || x.totalSpending.Trim().ToLower().Contains(kereses_tb.Text) || x.averageOrderValue.Trim().ToLower().Contains(kereses_tb.Text) || x.frequency.Trim().ToLower().Contains(kereses_tb.Text) || x.preferredCategory.Trim().ToLower().Contains(kereses_tb.Text) || x.churned.Trim().ToLower().Contains(kereses_tb.Text)).ToList();
            
            if (SearchList.Count == 0)
            {
                MessageBox.Show($"No resluts.");
                kereses_tb.Text = "";
            }


            page = 0;
            PageButtonRefresh();
            Load();
        }

        private void pageMinus_btn_Click(object sender, RoutedEventArgs e)
        {
            page--;
            PageButtonRefresh();
            Load();
        }

        private void pagePlusz_btn_Click(object sender, RoutedEventArgs e)
        {
            page++;
            PageButtonRefresh();
            Load();
        }
        private void LoadedpageMinus_btn_Click(object sender, RoutedEventArgs e)
        {
            Loadedpage--;
            LoadedPageButtonRefresh();
            Load();
        }

        private void LoadedpagePlusz_btn_Click(object sender, RoutedEventArgs e)
        {
            Loadedpage++;
            LoadedPageButtonRefresh();
            Load();
        }
        private void ErrorpageMinus_btn_Click(object sender, RoutedEventArgs e)
        {
            Errorpage--;
            ErrorPageButtonRefresh();
            Load();
        }

        private void ErrorpagePlusz_btn_Click(object sender, RoutedEventArgs e)
        {
            Errorpage++;
            ErrorPageButtonRefresh();
            Load();
        }

        private void PageButtonRefresh()
        {
            if (SearchList.Count > 0)
            {
                if (page == 0) pageMinus_btn.Visibility = Visibility.Hidden;
                else pageMinus_btn.Visibility = Visibility.Visible;
                if ((page * 25 + 25) >= SearchList.Count)
                {
                    pagePlusz_btn.Visibility = Visibility.Hidden;
                    pageNumber_lb.Content = $"{page * 25 + 1}-{SearchList.Count} customers";
                }
                else
                {
                    pagePlusz_btn.Visibility = Visibility.Visible;
                    pageNumber_lb.Content = $"{page * 25 + 1}-{page * 25 + 25} customers";
                }
            }
            else
            {
                if (page == 0) pageMinus_btn.Visibility = Visibility.Hidden;
                else pageMinus_btn.Visibility = Visibility.Visible;
                if ((page * 25 + 25) >= CustomersList.Count)
                {
                    pagePlusz_btn.Visibility = Visibility.Hidden;
                    pageNumber_lb.Content = $"{page * 25 + 1}-{CustomersList.Count} customers";
                }
                else
                {
                    pagePlusz_btn.Visibility = Visibility.Visible;
                    pageNumber_lb.Content = $"{page * 25 + 1}-{page * 25 + 25} customers";
                }
            }
        }

        private void LoadedPageButtonRefresh()
        {
            
            if (Loadedpage == 0) LoadedpageMinus_btn.Visibility = Visibility.Hidden;
            else LoadedpageMinus_btn.Visibility = Visibility.Visible;
            if ((Loadedpage * 25 + 25) >= LoadedFile.Count)
            {
                LoadedpagePlusz_btn.Visibility = Visibility.Hidden;
                LoadedpageNumber_lb.Content = $"{Loadedpage * 25 + 1}-{LoadedFile.Count} customers";
            }
            else
            {
                LoadedpagePlusz_btn.Visibility = Visibility.Visible;
                LoadedpageNumber_lb.Content = $"{Loadedpage * 25 + 1}-{Loadedpage * 25 + 25} customers";
            }
            
        }

        private void ErrorPageButtonRefresh()
        {

            if (Errorpage == 0) ErrorpageMinus_btn.Visibility = Visibility.Hidden;
            else ErrorpageMinus_btn.Visibility = Visibility.Visible;
            if ((Errorpage * 25 + 25) >= errors.Count)
            {
                ErrorpagePlusz_btn.Visibility = Visibility.Hidden;
                ErrorpageNumber_lb.Content = $"{Errorpage * 25 + 1}-{errors.Count} customers";
            }
            else
            {
                ErrorpagePlusz_btn.Visibility = Visibility.Visible;
                ErrorpageNumber_lb.Content = $"{Errorpage * 25 + 1}-{Errorpage * 25 + 25} customers";
            }

        }
    }
}
