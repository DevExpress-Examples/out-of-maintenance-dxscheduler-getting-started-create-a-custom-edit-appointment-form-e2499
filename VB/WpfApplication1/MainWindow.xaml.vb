Imports Microsoft.VisualBasic
Imports System.Windows
#Region "#usings"
Imports System.Data
Imports System.Data.OleDb
Imports DevExpress.XtraScheduler
Imports DevExpress.Xpf.Scheduler
#End Region '"#usings"

Namespace WpfApplication1
    Partial Public Class MainWindow
        Inherits Window

        Private dataSet As CarsDBDataSet
        Private adapter As CarsDBDataSetTableAdapters.CarSchedulingTableAdapter

        Public Sub New()
            InitializeComponent()

            CarSchedulingControl.Start = New System.DateTime(2010, 7, 15, 0, 0, 0, 0)

            Me.dataSet = New CarsDBDataSet()

            ' Bind the scheduler storage to appointments data.
            Me.CarSchedulingControl.Storage.AppointmentStorage.DataSource = dataSet.CarScheduling

            ' Load data into the 'CarsDBDataSet.CarScheduling' table. 
            Me.adapter = New CarsDBDataSetTableAdapters.CarSchedulingTableAdapter()
            Me.adapter.Fill(dataSet.CarScheduling)

            ' Bind the scheduler storage to resource data.
            Me.CarSchedulingControl.Storage.ResourceStorage.DataSource = dataSet.Cars

            ' Load data into the 'CarsDBDataSet.Cars' table.
            Dim carsAdapter As New CarsDBDataSetTableAdapters.CarsTableAdapter()
            carsAdapter.Fill(dataSet.Cars)

            AddHandler CarSchedulingControl.Storage.AppointmentsInserted,
                AddressOf Storage_AppointmentsModified
            AddHandler CarSchedulingControl.Storage.AppointmentsChanged,
                AddressOf Storage_AppointmentsModified
            AddHandler CarSchedulingControl.Storage.AppointmentsDeleted,
                AddressOf Storage_AppointmentsModified

            AddHandler adapter.Adapter.RowUpdated, AddressOf adapter_RowUpdated
        End Sub

        Private Sub Storage_AppointmentsModified(ByVal sender As Object, _
                                                 ByVal e As PersistentObjectsEventArgs)
            Me.adapter.Adapter.Update(Me.dataSet)
            Me.dataSet.AcceptChanges()

        End Sub

        Private Sub adapter_RowUpdated(ByVal sender As Object, ByVal e As OleDbRowUpdatedEventArgs)
            If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
                Dim id As Integer = 0
                Using cmd As New OleDbCommand("SELECT @@IDENTITY", adapter.Connection)
                    id = CInt(Fix(cmd.ExecuteScalar()))
                End Using
                e.Row("ID") = id
            End If
        End Sub
#Region "#editappointmentformshowing"
        Private Sub CarSchedulingControl_EditAppointmentFormShowing(sender As System.Object, _
                                                                    e As EditAppointmentFormEventArgs)
            e.Form = New CustomAppointmentForm(Me.CarSchedulingControl, e.Appointment)
        End Sub
#End Region '"#editappointmentformshowing"
    End Class
End Namespace

