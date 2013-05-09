using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Configuration.Install;

namespace AidSystemService
{
    [RunInstaller(true)]
    public class AidServiceEventLogInstaller : Installer
    {
        private EventLogInstaller _eventLogInstaller;
        public EventLogInstaller EventInstaller
        {
            get 
            {
                return _eventLogInstaller;
            }
        }

        public AidServiceEventLogInstaller()
		{
			//Create Instance of EventLogInstaller
            _eventLogInstaller = new EventLogInstaller();

			// Set the Source of Event Log, to be created.
			_eventLogInstaller.Source = "TEST";

			// Set the Log that source is created in
            _eventLogInstaller.Log = "xQuant.AidSystem";
			
			// Add myEventLogInstaller to the Installers Collection.
			//Installers.Add(eventLogInstaller);
		}

    }
}
