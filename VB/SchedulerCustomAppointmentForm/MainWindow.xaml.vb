Imports System.Windows
#Region "#usings"
Imports DevExpress.XtraScheduler
Imports DevExpress.Xpf.Scheduler
#End Region ' #usings

Namespace WpfApplication1
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Private dataSet As SchedulerTestDataSet
        Private adapter As SchedulerTestDataSetTableAdapters.AppointmentsTableAdapter
        Private resourcesAdapter As SchedulerTestDataSetTableAdapters.ResourcesTableAdapter

        Public Sub New()
            InitializeComponent()
            Me.dataSet = New SchedulerTestDataSet()

            ' Bind the scheduler storage to appointment data.   
            Me.Scheduler.Storage.AppointmentStorage.DataSource = dataSet.Appointments

            ' Load data to the 'CarsDBDataSet.CarScheduling' table.    
            Me.adapter = New SchedulerTestDataSetTableAdapters.AppointmentsTableAdapter()
            Me.adapter.Fill(dataSet.Appointments)

            ' Bind the scheduler storage to resource data.   
            Me.Scheduler.Storage.ResourceStorage.DataSource = dataSet.Resources

            ' Load data to the 'CarsDBDataSet.Cars' table.    
            Me.resourcesAdapter = New SchedulerTestDataSetTableAdapters.ResourcesTableAdapter()
            resourcesAdapter.Fill(dataSet.Resources)

            AddHandler Scheduler.Storage.AppointmentsInserted, AddressOf Storage_AppointmentsModified
            AddHandler Scheduler.Storage.AppointmentsChanged, AddressOf Storage_AppointmentsModified
            AddHandler Scheduler.Storage.AppointmentsDeleted, AddressOf Storage_AppointmentsModified

        End Sub



        Private Sub Storage_AppointmentsModified(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
            Me.adapter.Adapter.Update(Me.dataSet)
            Me.dataSet.AcceptChanges()

        End Sub


        #Region "#editappointmentformshowing"
        Private Sub Scheduler_EditAppointmentFormShowing(ByVal sender As Object, ByVal e As EditAppointmentFormEventArgs)
            e.Form = New CustomAppointmentForm(Scheduler, e.Appointment)
        End Sub
        #End Region ' #editappointmentformshowing
    End Class
End Namespace
