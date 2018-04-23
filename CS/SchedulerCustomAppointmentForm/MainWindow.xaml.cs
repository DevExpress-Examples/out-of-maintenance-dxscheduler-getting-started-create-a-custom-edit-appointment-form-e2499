using System.Windows;
#region #usings
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Scheduler;
#endregion #usings

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        SchedulerTestDataSet dataSet;
        SchedulerTestDataSetTableAdapters.AppointmentsTableAdapter adapter;
        SchedulerTestDataSetTableAdapters.ResourcesTableAdapter resourcesAdapter;

        public MainWindow() {
            InitializeComponent();
            this.dataSet = new SchedulerTestDataSet();

            // Bind the scheduler storage to appointment data.   
            this.Scheduler.Storage.AppointmentStorage.DataSource = dataSet.Appointments;

            // Load data to the 'CarsDBDataSet.CarScheduling' table.    
            this.adapter = new SchedulerTestDataSetTableAdapters.AppointmentsTableAdapter();
            this.adapter.Fill(dataSet.Appointments);

            // Bind the scheduler storage to resource data.   
            this.Scheduler.Storage.ResourceStorage.DataSource = dataSet.Resources;

            // Load data to the 'CarsDBDataSet.Cars' table.    
            this.resourcesAdapter = new SchedulerTestDataSetTableAdapters.ResourcesTableAdapter();
            resourcesAdapter.Fill(dataSet.Resources);

            this.Scheduler.Storage.AppointmentsInserted +=
                new PersistentObjectsEventHandler(Storage_AppointmentsModified);
            this.Scheduler.Storage.AppointmentsChanged +=
                new PersistentObjectsEventHandler(Storage_AppointmentsModified);
            this.Scheduler.Storage.AppointmentsDeleted +=
                new PersistentObjectsEventHandler(Storage_AppointmentsModified);
          
        }

       

        void Storage_AppointmentsModified(object sender, PersistentObjectsEventArgs e)
        {
            this.adapter.Adapter.Update(this.dataSet);
            this.dataSet.AcceptChanges();

        }
    

        #region #editappointmentformshowing
        private void Scheduler_EditAppointmentFormShowing(object sender, EditAppointmentFormEventArgs e)
        {
            e.Form = new CustomAppointmentForm(Scheduler, e.Appointment);
        }
        #endregion #editappointmentformshowing
    }
}
