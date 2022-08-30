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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Skillbox_Homework_13._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AddClientDialog addClientDialog = new AddClientDialog();
        OpenBillWindow openBillWindow = new OpenBillWindow();
        DeleteBillWindow deleteBillWindow = new DeleteBillWindow();
        AddMoneyWindow addMoneyWindow = new AddMoneyWindow();
        TransferMoneyWindow transferMoneyWindow = new TransferMoneyWindow();
        ObservableCollection<Client<Bill>> clients = new ObservableCollection<Client<Bill>>();
        ObservableCollection<ScreenInfo> screenInfo = new ObservableCollection<ScreenInfo>();

        float transferMoney = 0;

        public MainWindow()
        {
            InitializeComponent();
            listView.ItemsSource = screenInfo;
            this.Closed += new EventHandler(ClosedWindow);           
        }

        private void OpenAccount(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                openBillWindow = new OpenBillWindow();
                openBillWindow.buttonNewDeposit.Click += new RoutedEventHandler(NewDeposit);
                openBillWindow.buttonNewNonDeposit.Click += new RoutedEventHandler(NewNonDeposit);
                openBillWindow.Show();
                statusBar.Text = "";
            }
            else
            {
                statusBar.Text = "Выберите клиента для добавления счёта";
            }
        }

        private void CloseAccount(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                deleteBillWindow = new DeleteBillWindow();
                deleteBillWindow.buttonDeleteDeposit.Click += new RoutedEventHandler(DeleteDeposit);
                deleteBillWindow.buttonDeleteNonDeposit.Click += new RoutedEventHandler(DeleteNonDeposit);
                deleteBillWindow.Show();
                statusBar.Text = "";
            }
            else
            {
                statusBar.Text = "Выберите клиента для удаления счёта";
            }
        }

        private void TransferMoney(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                transferMoneyWindow = new TransferMoneyWindow();
                transferMoneyWindow.listView.ItemsSource = screenInfo;
                transferMoneyWindow.listView.SelectionChanged += new SelectionChangedEventHandler(RenewBillsSelection);
                transferMoneyWindow.buttonProceed.Click += new RoutedEventHandler(ButtonProceed);

                transferMoneyWindow.textblockSecondName.Text = clients[listView.SelectedIndex].SecondName;
                transferMoneyWindow.textblockFirstName.Text = clients[listView.SelectedIndex].FirstName;
                transferMoneyWindow.textblockPatronymic.Text = clients[listView.SelectedIndex].Patronymic;

                transferMoneyWindow.textblockBillName1.Text = clients[listView.SelectedIndex].bill1.Name;
                transferMoneyWindow.textblockBillName2.Text = clients[listView.SelectedIndex].bill2.Name;

                if (clients[listView.SelectedIndex].bill1.GetType() == typeof(DefaultBill))
                {
                    transferMoneyWindow.radioReceiverBill1.IsEnabled = false; 
                }
                else
                {
                    transferMoneyWindow.radioReceiverBill1.IsEnabled = true;
                }

                if (clients[listView.SelectedIndex].bill2.GetType() == typeof(DefaultBill))
                {
                    transferMoneyWindow.radioReceiverBill2.IsEnabled = false;
                }
                else
                {
                    transferMoneyWindow.radioReceiverBill2.IsEnabled = true;
                }              

                transferMoneyWindow.Show();
            }
            else
            {
                statusBar.Text = "Выберите клиента для перевода денег счёта";
            }
        }

        private void RenewBillsSelection(object sender, RoutedEventArgs e)
        {
            try
            {
                transferMoneyWindow.clientbill1Name.Text = clients[transferMoneyWindow.listView.SelectedIndex].bill1.Name;
                transferMoneyWindow.clientbill2Name.Text = clients[transferMoneyWindow.listView.SelectedIndex].bill2.Name;

                transferMoneyWindow.clientbill1Ammount.Text = Convert.ToString(clients[transferMoneyWindow.listView.SelectedIndex].bill1.Ammount);
                transferMoneyWindow.clientbill2Ammount.Text = Convert.ToString(clients[transferMoneyWindow.listView.SelectedIndex].bill2.Ammount);

                if (clients[transferMoneyWindow.listView.SelectedIndex].bill1.GetType() == typeof(DefaultBill))
                {
                    transferMoneyWindow.radioSenderBill1.IsEnabled = false;
                }
                else
                {
                    transferMoneyWindow.radioSenderBill1.IsEnabled = true;
                }

                if (clients[transferMoneyWindow.listView.SelectedIndex].bill2.GetType() == typeof(DefaultBill))
                {
                    transferMoneyWindow.radioSenderBill2.IsEnabled = false;
                }
                else
                {
                    transferMoneyWindow.radioSenderBill2.IsEnabled = true;
                }
            }
            catch
            {

            }                        
        }

        private void ButtonProceed(object sender, RoutedEventArgs e)
        {
            if (transferMoneyWindow.textboxTransferAmmount.Text != "" && float.Parse(transferMoneyWindow.textboxTransferAmmount.Text) >= 0)
            {               
                if (Convert.ToBoolean(transferMoneyWindow.radioSenderBill1.IsChecked))
                {
                    if (clients[transferMoneyWindow.listView.SelectedIndex].bill1.Ammount >= float.Parse(transferMoneyWindow.textboxTransferAmmount.Text))
                    {
                        IShowBill<Bill> showBill = clients[transferMoneyWindow.listView.SelectedIndex];
                        transferMoney = float.Parse(transferMoneyWindow.textboxTransferAmmount.Text);

                        showBill.ShowBill(1).Ammount -= transferMoney;
                    }
                }

                if (Convert.ToBoolean(transferMoneyWindow.radioSenderBill2.IsChecked))
                {
                    if (clients[transferMoneyWindow.listView.SelectedIndex].bill2.Ammount >= float.Parse(transferMoneyWindow.textboxTransferAmmount.Text))
                    {
                        IShowBill<Bill> showBill = clients[transferMoneyWindow.listView.SelectedIndex];
                        transferMoney = float.Parse(transferMoneyWindow.textboxTransferAmmount.Text);

                        showBill.ShowBill(2).Ammount -= transferMoney;
                    }
                }

                Debug.WriteLine($" Трансфер мани: {transferMoney}");
                
                if (Convert.ToBoolean(transferMoneyWindow.radioReceiverBill1.IsChecked))
                {
                    IShowBill<Bill> showBill = clients[listView.SelectedIndex];
                    showBill.ShowBill(1).Ammount = showBill.ShowBill(1).Ammount + transferMoney;
                }

                if (Convert.ToBoolean(transferMoneyWindow.radioReceiverBill2.IsChecked))
                {
                    IShowBill<Bill> showBill = clients[listView.SelectedIndex];
                    showBill.ShowBill(2).Ammount = showBill.ShowBill(2).Ammount + transferMoney; 
                }

                transferMoneyWindow.Close();

                VisibleInformation();
            }
            else
            {
                MessageBox.Show("Поле не может быть пустым. Сумма не может быть отрицательной");
            }
        }

        private void AddMoney(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                addMoneyWindow = new AddMoneyWindow();
                addMoneyWindow.buttonAdd.Click += new RoutedEventHandler(ButtonAddMoney);

                if (clients[listView.SelectedIndex].bill1.GetType() != typeof(DefaultBill))
                {
                    addMoneyWindow.textbox1.IsEnabled = true;
                }
                else
                {
                    addMoneyWindow.textbox1.IsEnabled = false;
                }

                if (clients[listView.SelectedIndex].bill2.GetType() != typeof(DefaultBill))
                {
                    addMoneyWindow.textbox2.IsEnabled = true;
                }
                else
                {
                    addMoneyWindow.textbox2.IsEnabled = false;
                }

                if (clients[listView.SelectedIndex].bill1.GetType() == typeof(DefaultBill) 
                    && clients[listView.SelectedIndex].bill2.GetType() == typeof(DefaultBill))
                {
                    statusBar.Text = "У клиента нет существующих счетов";
                }
                else
                {
                    addMoneyWindow.Show();
                    statusBar.Text = "";
                }
            }
            else
            {
                statusBar.Text = "Выберите клиента";
            }
        }

        private void ButtonAddMoney(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addMoneyWindow.textbox1.Text == "")
                {
                    addMoneyWindow.textbox1.Text = "0";
                }

                if (addMoneyWindow.textbox2.Text == "")
                {
                    addMoneyWindow.textbox2.Text = "0";
                }

                if (float.Parse(addMoneyWindow.textbox1.Text) < 0 || float.Parse(addMoneyWindow.textbox2.Text) < 0)
                {
                    statusBar.Text = "Сумма пополнения не может быть отрицательной";
                }
                else
                {
                    clients[listView.SelectedIndex].bill1.Ammount += float.Parse(addMoneyWindow.textbox1.Text);
                    clients[listView.SelectedIndex].bill2.Ammount += float.Parse(addMoneyWindow.textbox2.Text);

                    VisibleInformation();
                }

                addMoneyWindow.Close();
            }
            catch (Exception)
            {
                throw new Exception("Недопустимо использовать что то кроме цифр. Для ввода дробной части цифры воспользуйтель знаком запятой вместо точки");
            }
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            addClientDialog = new AddClientDialog();
            addClientDialog.buttonAddClient.Click += new RoutedEventHandler(ButtonAddClient);
            addClientDialog.Show();
        }

        private void ButtonAddClient(object sender, RoutedEventArgs e)
        {
            if (addClientDialog.textboxSecondName.Text == "" || addClientDialog.textboxFirstName.Text == "" || addClientDialog.textboxPatronymic.Text == "")
            {
                MessageBox.Show("Поля не могут быть пустыми");
            }
            else
            {
                var temp = new Client<Bill>(addClientDialog.textboxSecondName.Text, addClientDialog.textboxFirstName.Text, addClientDialog.textboxPatronymic.Text);
                temp.bill1 = new DefaultBill("Счёт не создан", 0);
                temp.bill2 = new DefaultBill("Счёт не создан", 0);

                clients.Add(temp);
                addClientDialog.Close();

                VisibleInformation();
                statusBar.Text = "";
            }
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewDeposit(object sender, RoutedEventArgs e)
        {
            if (clients[listView.SelectedIndex].bill2.GetType() == typeof(DefaultBill))
            {
                IChangeBill<DepositBill> temp = clients[listView.SelectedIndex];
                temp.ChangeBill(new DepositBill("Депозитный счёт", 0), 2);

                openBillWindow.Close();

                VisibleInformation();
                statusBar.Text = "";
            }
            else
            {
                MessageBox.Show("Счёт уже существует");
            }
        }
         
        private void NewNonDeposit(object sender, RoutedEventArgs e)
        {
            if (clients[listView.SelectedIndex].bill1.GetType() == typeof(DefaultBill))
            {
                IChangeBill<NonDepositBill> temp = clients[listView.SelectedIndex];
                temp.ChangeBill(new NonDepositBill("Недепозитный счёт", 0), 1);

                openBillWindow.Close();

                VisibleInformation();
                statusBar.Text = "";
            }
            else
            {
                MessageBox.Show("Счёт уже существует");
            }            
        }

        private void DeleteDeposit(object sender, RoutedEventArgs e)
        {
            if (clients[listView.SelectedIndex].bill2.GetType() != typeof(DefaultBill))
            {
                if (clients[listView.SelectedIndex].bill2.Ammount > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Счёт не пустой, все деньги будут удалены. Желаете продолжить?", "Предупреждение", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        IChangeBill<DefaultBill> temp = clients[listView.SelectedIndex];
                        temp.ChangeBill(new DefaultBill("Счёт не создан", 0), 2);

                        deleteBillWindow.Close();

                        VisibleInformation();
                    }
                    else
                    {

                    }
                }
                else
                {
                    IChangeBill<DefaultBill> temp = clients[listView.SelectedIndex];
                    temp.ChangeBill(new DefaultBill("Счёт не создан", 0), 2);

                    deleteBillWindow.Close();

                    VisibleInformation();
                }
                
                statusBar.Text = "";
            }
            else
            {
                MessageBox.Show("Счёт уже не существует");
            }
        }

        private void DeleteNonDeposit(object sender, RoutedEventArgs e)
        {
            if (clients[listView.SelectedIndex].bill1.GetType() != typeof(DefaultBill))
            {
                if (clients[listView.SelectedIndex].bill1.Ammount > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Счёт не пустой, все деньги будут удалены. Желаете продолжить?", "Предупреждение", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        IChangeBill<DefaultBill> temp = clients[listView.SelectedIndex];
                        temp.ChangeBill(new DefaultBill("Счёт не создан", 0), 1);

                        deleteBillWindow.Close();

                        VisibleInformation();
                    }
                    else
                    {

                    }
                }
                else
                {
                    IChangeBill<DefaultBill> temp = clients[listView.SelectedIndex];
                    temp.ChangeBill(new DefaultBill("Счёт не создан", 0), 1);

                    deleteBillWindow.Close();

                    VisibleInformation();
                }
                
                statusBar.Text = "";
            }
            else
            {
                MessageBox.Show("Счёт уже не существует");
            }
        }

        private void VisibleInformation()
        {
            screenInfo.Clear();

            foreach (var item in clients)
            {
                screenInfo.Add(new ScreenInfo(item.SecondName, 
                                              item.FirstName, 
                                              item.Patronymic, 
                                              item.bill1.Name, 
                                              Convert.ToString(item.bill1.Ammount), 
                                              item.bill2.Name, 
                                              Convert.ToString(item.bill2.Ammount)));
            }
        }
    }
}
