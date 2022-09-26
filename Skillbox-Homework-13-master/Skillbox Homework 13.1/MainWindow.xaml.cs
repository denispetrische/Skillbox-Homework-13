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
using Skillbox_Homework_13._1.Classes;
using Skillbox_Homework_13._1.Interfaces;
using Skillbox_Homework_13._1.Displays;

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
        NotificationWindow notificationWindow = new NotificationWindow();

        ObservableCollection<Client> clients = new ObservableCollection<Client>();
        ObservableCollection<ScreenInfo> screenInfo = new ObservableCollection<ScreenInfo>();

        public ObservableCollection<string> clientInformation = new ObservableCollection<string>();

        private delegate void someInfo(string message);
        private event someInfo? someMessage; 


        public MainWindow()
        {
            InitializeComponent();
            listView.ItemsSource = screenInfo;
            this.Closed += new EventHandler(ClosedWindow);
            Client.ClientNotification += NotificationHandler;
            someMessage += NotificationHandler;
        }

        private void OpenAccount(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для добавления счёта");
            }
            else
            {
                openBillWindow = new OpenBillWindow();
                openBillWindow.buttonNewDeposit.Click += new RoutedEventHandler(NewDeposit);
                openBillWindow.buttonNewNonDeposit.Click += new RoutedEventHandler(NewNonDeposit);
                openBillWindow.Show();
            }
        }

        private void NewDeposit(object sender, RoutedEventArgs e)
        {
            clients[listView.SelectedIndex].depositBill = new DepositBill(0);

            someMessage?.Invoke($"{DateTime.Now} У клиента {clients[listView.SelectedIndex].SecondName} {clients[listView.SelectedIndex].FirstName} {clients[listView.SelectedIndex].Patronymic} открыт новый депозитный счёт");

            VisibleInformation();

            openBillWindow.Close();
        }

        private void NewNonDeposit(object sender, RoutedEventArgs e)
        {
            clients[listView.SelectedIndex].nonDepositBill = new NonDepositBill(0);

            someMessage?.Invoke($"{DateTime.Now} У клиента {clients[listView.SelectedIndex].SecondName} {clients[listView.SelectedIndex].FirstName} {clients[listView.SelectedIndex].Patronymic} открыт новый недепозитный счёт");

            VisibleInformation();

            openBillWindow.Close();
        }

        private void CloseAccount(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для удаления счёта");
            }
            else
            {
                deleteBillWindow = new DeleteBillWindow();
                deleteBillWindow.buttonDeleteDeposit.Click += new RoutedEventHandler(DeleteDeposit);
                deleteBillWindow.buttonDeleteNonDeposit.Click += new RoutedEventHandler(DeleteNonDeposit);
                deleteBillWindow.Show();
            }
        }

        private void DeleteDeposit(object sender, RoutedEventArgs e)
        {
            clients[listView.SelectedIndex].depositBill = new DefaultBill(0);

            someMessage?.Invoke($"{DateTime.Now} У клиента {clients[listView.SelectedIndex].SecondName} {clients[listView.SelectedIndex].FirstName} {clients[listView.SelectedIndex].Patronymic} закрыт депозитный счёт");

            VisibleInformation();

            deleteBillWindow.Close();
        }

        private void DeleteNonDeposit(object sender, RoutedEventArgs e)
        {
            clients[listView.SelectedIndex].nonDepositBill = new DefaultBill(0);

            someMessage?.Invoke($"{DateTime.Now} У клиента {clients[listView.SelectedIndex].SecondName} {clients[listView.SelectedIndex].FirstName} {clients[listView.SelectedIndex].Patronymic} закрыт недепозитный счёт");

            VisibleInformation();

            deleteBillWindow.Close();
        }

        private void TransferMoney(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для перевода денег");
            }
            else
            {
                if (clients[listView.SelectedIndex].depositBill.GetType() == typeof(DefaultBill) && clients[listView.SelectedIndex].nonDepositBill.GetType() == typeof(DefaultBill))
                {
                    MessageBox.Show("У выбранного клиента не существует счетов");
                }
                else
                {
                    transferMoneyWindow = new TransferMoneyWindow();
                    transferMoneyWindow.buttonProceed.Click += new RoutedEventHandler(ButtonTransfer);
                    transferMoneyWindow.listView.SelectionChanged += new SelectionChangedEventHandler(TransferListviewChanged);

                    transferMoneyWindow.textblockSecondName.Text = clients[listView.SelectedIndex].SecondName;
                    transferMoneyWindow.textblockFirstName.Text = clients[listView.SelectedIndex].FirstName;
                    transferMoneyWindow.textblockPatronymic.Text = clients[listView.SelectedIndex].Patronymic;

                    if (clients[listView.SelectedIndex].depositBill.GetType() == typeof(DefaultBill))
                    {
                        transferMoneyWindow.textblockBillName1.Text = "Счёт не создан";
                        transferMoneyWindow.radioReceiverBill1.IsEnabled = false;
                    }
                    else
                    {
                        transferMoneyWindow.textblockBillName1.Text = "Депозитный счёт";
                    }

                    if (clients[listView.SelectedIndex].nonDepositBill.GetType() == typeof(DefaultBill))
                    {
                        transferMoneyWindow.textblockBillName2.Text = "Счёт не создан";
                        transferMoneyWindow.radioReceiverBill2.IsEnabled = false;
                    }
                    else
                    {
                        transferMoneyWindow.textblockBillName2.Text = "Недепозитный счёт";
                    }

                    transferMoneyWindow.listView.ItemsSource = screenInfo;

                    transferMoneyWindow.Show();
                }               
            }
        }

        private void TransferListviewChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (clients[transferMoneyWindow.listView.SelectedIndex].depositBill.GetType() == typeof(DefaultBill))
                {
                    transferMoneyWindow.clientbill1Name.Text = "Счёт не создан";
                    transferMoneyWindow.radioSenderBill1.IsEnabled = false;
                }
                else
                {
                    transferMoneyWindow.clientbill1Name.Text = "Депозитный счёт";
                    transferMoneyWindow.clientbill1Ammount.Text = $"{clients[transferMoneyWindow.listView.SelectedIndex].depositBill.Ammount}";
                    transferMoneyWindow.radioSenderBill1.IsEnabled = true;
                }

                if (clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill.GetType() == typeof(DefaultBill))
                {
                    transferMoneyWindow.clientbill2Name.Text = "Счёт не создан";
                    transferMoneyWindow.radioSenderBill2.IsEnabled = false;
                }
                else
                {
                    transferMoneyWindow.clientbill2Name.Text = "Недепозитный счёт";
                    transferMoneyWindow.clientbill2Ammount.Text = $"{clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill.Ammount}";
                    transferMoneyWindow.radioSenderBill2.IsEnabled = true;
                }
            }
            catch 
            {

            }
            
        }

        private void ButtonTransfer(object sender, RoutedEventArgs e)
        {
            if (transferMoneyWindow.listView.SelectedItem != null)
            {
                if (transferMoneyWindow.radioSenderBill1.IsEnabled == false && transferMoneyWindow.radioSenderBill2.IsEnabled == false)
                {
                    MessageBox.Show("У выбранного клиента нет счетов с которых можно перевести деньги");
                }
                else
                {
                    if (Convert.ToBoolean(transferMoneyWindow.radioReceiverBill1.IsChecked))
                    {
                        if (Convert.ToBoolean(transferMoneyWindow.radioSenderBill1.IsChecked))
                        {
                            clients[listView.SelectedIndex] = clients[listView.SelectedIndex].Transfer(clients[transferMoneyWindow.listView.SelectedIndex].depositBill,
                                                                 Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text),
                                                                 "DepositBill");

                            if (clients[transferMoneyWindow.listView.SelectedIndex].depositBill.Ammount >= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text))
                            {
                                clients[transferMoneyWindow.listView.SelectedIndex].depositBill.Ammount -= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text);
                            }
                        }

                        if (Convert.ToBoolean(transferMoneyWindow.radioSenderBill2.IsChecked))
                        {
                            clients[listView.SelectedIndex] = clients[listView.SelectedIndex].Transfer(clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill,
                                                                 Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text),
                                                                 "DepositBill");

                            if (clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill.Ammount >= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text))
                            {
                                clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill.Ammount -= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text);
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(transferMoneyWindow.radioSenderBill1.IsChecked))
                        {
                            clients[listView.SelectedIndex] = clients[listView.SelectedIndex].Transfer(clients[transferMoneyWindow.listView.SelectedIndex].depositBill,
                                                                 Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text),
                                                                 "NonDepositBill");

                            if (clients[transferMoneyWindow.listView.SelectedIndex].depositBill.Ammount >= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text))
                            {
                                clients[transferMoneyWindow.listView.SelectedIndex].depositBill.Ammount -= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text);
                            }
                        }

                        if (Convert.ToBoolean(transferMoneyWindow.radioSenderBill2.IsChecked))
                        {
                            clients[listView.SelectedIndex] = clients[listView.SelectedIndex].Transfer(clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill,
                                                                 Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text),
                                                                 "NonDepositBill");

                            if (clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill.Ammount >= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text))
                            {
                                clients[transferMoneyWindow.listView.SelectedIndex].nonDepositBill.Ammount -= Convert.ToSingle(transferMoneyWindow.textboxTransferAmmount.Text);
                            }
                        }
                    }                  
                }

                transferMoneyWindow.Close();            

                VisibleInformation();
            }
            else
            {
                MessageBox.Show("Выберите клиента с чьего счёта вы хотите перевести деньги");
            }
        }

        private void AddMoney(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для пополнения счёта");
            }
            else
            {
                addMoneyWindow = new AddMoneyWindow();
                addMoneyWindow.buttonAdd.Click += new RoutedEventHandler(ButtonAddMoney);

                if (clients[listView.SelectedIndex].depositBill.GetType() == typeof(DefaultBill))
                {
                    addMoneyWindow.textbox1.IsEnabled = false;
                }

                if (clients[listView.SelectedIndex].nonDepositBill.GetType() == typeof(DefaultBill))
                {
                    addMoneyWindow.textbox2.IsEnabled = false;
                }

                addMoneyWindow.Show();
            }
        }

        private void ButtonAddMoney(object sender, RoutedEventArgs e)
        {
            if (addMoneyWindow.textbox1.IsEnabled)
            {
                try
                {
                    IAddMoney<Bill> temp1 = clients[listView.SelectedIndex].depositBill;
                    clients[listView.SelectedIndex].depositBill = temp1.AddMoney(Convert.ToSingle(addMoneyWindow.textbox1.Text));

                    someMessage?.Invoke($"{DateTime.Now} Клиенту {clients[listView.SelectedIndex].SecondName} {clients[listView.SelectedIndex].FirstName} {clients[listView.SelectedIndex].Patronymic} на депозитный счёт поступило {addMoneyWindow.textbox1.Text} условных единиц");
                }
                catch 
                {
                    MessageBox.Show("Введите число");
                }
                finally
                {

                }
            }

            if (addMoneyWindow.textbox2.IsEnabled)
            {
                try
                {
                    IAddMoney<Bill> temp2 = clients[listView.SelectedIndex].nonDepositBill;
                    clients[listView.SelectedIndex].nonDepositBill = temp2.AddMoney(Convert.ToSingle(addMoneyWindow.textbox2.Text));

                    someMessage?.Invoke($"{DateTime.Now} Клиенту {clients[listView.SelectedIndex].SecondName} {clients[listView.SelectedIndex].FirstName} {clients[listView.SelectedIndex].Patronymic} на недепозитный счёт поступило {addMoneyWindow.textbox2.Text} условных единиц");
                }
                catch
                {
                    MessageBox.Show("Введите число");
                }
                finally
                {

                }
            }

            VisibleInformation();

            addMoneyWindow.Close();
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
                clients.Add(new Client(addClientDialog.textboxSecondName.Text, addClientDialog.textboxFirstName.Text, addClientDialog.textboxPatronymic.Text));

                VisibleInformation();

                addClientDialog.Close();
            }
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void VisibleInformation()
        {
            screenInfo.Clear();

            string depositBillAmmount;
            string nonDepositBillAmmount;

            foreach (var item in clients)
            {
                if (item.depositBill.GetType() == typeof(DefaultBill))
                {
                    depositBillAmmount = "Счёт не создан";
                }
                else
                {
                    depositBillAmmount = $"{item.depositBill.Ammount}";
                }

                if (item.nonDepositBill.GetType() == typeof(DefaultBill))
                {
                    nonDepositBillAmmount = "Счёт не создан";
                }
                else
                {
                    nonDepositBillAmmount = $"{item.nonDepositBill.Ammount}";
                }

                screenInfo.Add(new ScreenInfo(item.SecondName, item.FirstName, item.Patronymic, depositBillAmmount, nonDepositBillAmmount));
            }
        }

        public void NotificationHandler(string message) => clientInformation.Add(message);

        private void OpenNotificationWindow(object sender, RoutedEventArgs e)
        {
            notificationWindow = new NotificationWindow();
            notificationWindow.listView.ItemsSource = clientInformation;
            notificationWindow.Show();
        }
    }
}
