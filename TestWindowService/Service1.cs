using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using GSB_Csharp;

namespace TestWindowService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer1 = null;

        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ouverture du service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 30000;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            Library.WriteErrorLog("Test Window service started");
        }

        //Evenement Tick du timer qui fait à intervalle de temps régulier ,les mises à jour dans la base GSB
        private void timer1_Tick(object sender,ElapsedEventArgs e)
        {
            TraitementBDD connect = new TraitementBDD("127.0.0.1", "gsb_frais", "root", "");

            //Ne pas oublier de fermer la connection

            string anne = DateTime.Now.Year.ToString();
            string mois = GestionDates.getMoisPrecedent();

            int anneInt;
            Int32.TryParse(anne, out anneInt);
            if (mois == "12")
            {
                //alors on a changé d'année 
                anneInt = anneInt - 1;
            }
            connect.execution("update fichefrais set idetat='RB' where idetat = 'VA' and mois = " + anneInt + mois + " ;");
            connect.execution("update fichefrais set idetat='CL' where idetat = 'CR' and mois = " + anneInt + mois + " ;");

            //-------------------------------------------------------------------------------------



            connect.fermeture();

            Library.WriteErrorLog("Opération réussitre");
        }

        /// <summary>
        /// Fermeture du service
        /// </summary>
        protected override void OnStop()
        {
            this.timer1.Enabled = false;
            Library.WriteErrorLog("Test window service stopped");
        }
    }
}
