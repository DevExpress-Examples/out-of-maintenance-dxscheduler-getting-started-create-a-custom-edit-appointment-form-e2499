Imports System
Imports System.Globalization
Imports System.Windows.Controls
#Region "#usings_form"
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.Xpf.Scheduler.UI
Imports DevExpress.XtraScheduler
#End Region ' #usings_form

Namespace WpfApplication1
    ''' <summary>
    ''' Interaction logic for CustomAppointmentForm.xaml
    ''' </summary>
#Region "#customform"
    Partial Public Class CustomAppointmentForm
        Inherits UserControl

        Private privateController As CustomAppointmentFormController
        Public Property Controller() As CustomAppointmentFormController
            Get
                Return privateController
            End Get
            Private Set(ByVal value As CustomAppointmentFormController)
                privateController = value
            End Set
        End Property
        Private privateControl As SchedulerControl
        Public Property Control() As SchedulerControl
            Get
                Return privateControl
            End Get
            Private Set(ByVal value As SchedulerControl)
                privateControl = value
            End Set
        End Property
        Private privateAppointment As Appointment
        Public Property Appointment() As Appointment
            Get
                Return privateAppointment
            End Get
            Private Set(ByVal value As Appointment)
                privateAppointment = value
            End Set
        End Property
        Public ReadOnly Property TimeEditMask() As String
            Get
                Return CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern
            End Get
        End Property


        Public Sub New(ByVal control As SchedulerControl, ByVal apt As Appointment)
            InitializeComponent()
            If control Is Nothing OrElse apt Is Nothing Then
                Throw New ArgumentNullException("control")
            End If
            If control Is Nothing OrElse apt Is Nothing Then
                Throw New ArgumentNullException("apt")
            End If

            Me.Control = control
            Me.Controller = New CustomAppointmentFormController(control, apt)
            Me.Appointment = apt
        End Sub

        Private Sub UserControl_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            If Controller.IsNewAppointment Then
                SchedulerFormBehavior.SetTitle(Me, "New appointment")
            Else
                SchedulerFormBehavior.SetTitle(Me, "Edit - [" & Appointment.Subject & "]")
            End If
        End Sub

        Private Sub Ok_button_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            ' Save all changes made to the appointment.            
            Controller.Control.Storage.BeginUpdate()
            Controller.ApplyChanges()
            Controller.Control.Storage.EndUpdate()
            SchedulerFormBehavior.Close(Me, True)
        End Sub

        Private Sub Cancel_button_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            SchedulerFormBehavior.Close(Me, False)
        End Sub
    End Class
#End Region ' #customform

#Region "#controller"
    Public Class CustomAppointmentFormController
        Inherits AppointmentFormController

        Public Sub New(ByVal control As SchedulerControl, ByVal apt As Appointment)
            MyBase.New(control, apt)
        End Sub

        Public Property Contact() As String
            Get
                Return GetContactValue(EditedAppointmentCopy)
            End Get
            Set(ByVal value As String)
                EditedAppointmentCopy.CustomFields("Contact") = value
            End Set
        End Property

        Private Property SourceContact() As String
            Get
                Return GetContactValue(SourceAppointment)
            End Get
            Set(ByVal value As String)
                SourceAppointment.CustomFields("Contact") = value
            End Set
        End Property

        Public Overrides Function IsAppointmentChanged() As Boolean
            If MyBase.IsAppointmentChanged() Then
                Return True
            End If
            Return SourceContact <> Contact
        End Function

        Protected Function GetContactValue(ByVal apt As Appointment) As String
            Return Convert.ToString(apt.CustomFields("Contact"))
        End Function
    End Class
#End Region ' #controller
End Namespace