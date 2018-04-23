using System;
using System.Globalization;
using System.Windows.Controls;
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.UI;

namespace WpfApplication1 {
    public partial class CustomAppointmentForm : UserControl {

        SchedulerControl control;
        CustomAppointmentFormController controller;
        public string TimeEditMask {
            get { return CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern; }
        }

        public CustomAppointmentForm(SchedulerControl control, Appointment apt) {
            InitializeComponent();
            if (control == null || apt == null)
                throw new ArgumentNullException("control");
            if (control == null || apt == null)
                throw new ArgumentNullException("apt");

            this.control = control;
            this.controller = new CustomAppointmentFormController(control, apt);

        }
        public CustomAppointmentFormController Controller { get { return controller; } }
        public SchedulerControl Control { get { return control; } }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            FormOperationHelper.SetFormCaption(this, "Custom Appointment Form");
        }

        private void Ok_button_Click(object sender, System.Windows.RoutedEventArgs e) {
            // Save all changes of the editing appointment.            
            Controller.ApplyChanges();
            FormOperationHelper.CloseDialog(this, true);
        }

        private void Cancel_button_Click(object sender, System.Windows.RoutedEventArgs e) {
            FormOperationHelper.CloseDialog(this, false);
        }
    }
}

    public class CustomAppointmentFormController : AppointmentFormController {
        public CustomAppointmentFormController(SchedulerControl control, Appointment apt)
            : base(control, apt) {
        }
        public string Contact {
            get { return GetContactValue(EditedAppointmentCopy); }
            set { EditedAppointmentCopy.CustomFields["Contact"] = value; }
        }
        string SourceContact {
            get { return GetContactValue(SourceAppointment); }
            set { SourceAppointment.CustomFields["Contact"] = value; }
        }

        public override bool IsAppointmentChanged() {
            if (base.IsAppointmentChanged())
                return true;
            return SourceContact != Contact;
        }

        protected string GetContactValue(Appointment apt) {
            return Convert.ToString(apt.CustomFields["Contact"]);
        }

        protected override void ApplyCustomFieldsValues() {
            SourceContact = Contact;
        }
    }