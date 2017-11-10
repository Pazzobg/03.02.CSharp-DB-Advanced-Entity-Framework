namespace P01_HospitalDatabase
{
    using System;
    using P01_HospitalDatabase.Data;
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new HospitalContext())
            {
                ResetDatabase(db);




            }
        }

        private static void ResetDatabase(HospitalContext db)
        {
            db.Database.EnsureDeleted();

            db.Database.Migrate();

            Seed(db);
        }

        private static void Seed(HospitalContext db)
        {
            var patients = new[]
            {
                new Patient("Ivan", "Ivanov", "...Ivan's address...", "ivan@abv.bg", true),
                new Patient("Petar", "Petrov", "...Petar's address...", "pesho@abv.bg", true),
                new Patient("Stoyan", "Stoyanov", "...Stoyan's address...", "stoyan@abv.bg", true),
                new Patient("Minka", "Petkova", "...Minka's address...", "minka@abv.bg", true)
            };

            db.Patients.AddRange(patients);

            var doctors = new[]
            {
                new Doctor("Oh, boli", "Тraumatologist"), 
                new Doctor("Zhivago", "Physician"), 
                new Doctor("Strange", "Surgeon"), 
                new Doctor("Who", "Neurologist"),
            };

            db.Doctors.AddRange(doctors);

            var date1 = new DateTime(2017, 06, 02);
            var date2 = new DateTime(2017, 06, 03);
            var date3 = new DateTime(2017, 07, 17);
            var date4 = new DateTime(2017, 08, 09);
            var visits = new[]
            {

                new Visitation(date1, "Needs more meds", patients[0], doctors[0]),
                new Visitation(date1, "Needs more meds", patients[1], doctors[0]),
                new Visitation(date1, "Needs less meds", patients[2], doctors[0]),
                new Visitation(date1, "Needs more meds", patients[3], doctors[0]),
                new Visitation(date2, "Patient checked, feels well", patients[0], doctors[1]),
                new Visitation(date2, "Patient checked, feels not so well", patients[1], doctors[1]),
                new Visitation(date2, "Patient checked, feels very well", patients[2], doctors[1]),
                new Visitation(date2, "Patient checked, feels well", patients[3], doctors[1]),
                new Visitation(date3, "Discharging soon", patients[0], doctors[2]),
                new Visitation(date3, "Good improvement", patients[1], doctors[2]),
                new Visitation(date3, "Good for discharging", patients[2], doctors[2]),
                new Visitation(date3, "Good for discharging", patients[3], doctors[2]),
                new Visitation(date4, "Good for discharging", patients[0], doctors[3]),
                new Visitation(date4, "Needs further treatment", patients[1], doctors[3])
            };

            db.Visitations.AddRange(visits);

            var diagnoses = new[]
            {
                new Diagnose("Broken limb", "Arm or Leg", patients[0]),
                new Diagnose("Respiratory problem", "Complexity?", patients[1]),
                new Diagnose("Neuro problems", "Specify...", patients[2]),
                new Diagnose("Ear infection", "Complexity?", patients[3]),
            };

            db.Diagnoses.AddRange(diagnoses);

            var meds = new[]
            {
                new Medicament("Gips"),
                new Medicament("Broncholytin"),
                new Medicament("Good long Rest..."),
                new Medicament("Otipax"),
            };

            db.Medicaments.AddRange(meds);

            var prescriptions = new[]
            {
                new PatientMedicament(){ PatientId = 1, Medicament = meds[0]},
                new PatientMedicament(){ PatientId = 2, Medicament = meds[1]},
                new PatientMedicament(){ PatientId = 3, Medicament = meds[2]},
                new PatientMedicament(){ PatientId = 4, Medicament = meds[3]},
            };

            db.PatientMedicament.AddRange(prescriptions);

            db.SaveChanges();
        }
    }
}
