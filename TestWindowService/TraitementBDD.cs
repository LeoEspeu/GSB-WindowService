using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//On utilise le package nugget Mysql.Data
using MySql.Data.MySqlClient;
using TestWindowService;

namespace GSB_Csharp
{
    /// <summary>
    /// Classe permettant de réaliser les traitements vers la base de données
    /// </summary>
    public class TraitementBDD
    {
        /// <summary>
        /// Objet réalisant les transactions après mise à jour de la base de données
        /// </summary>
        private MySqlTransaction uneTransaction;

        /// <summary>
        /// Objet réalisant la connection vers la base de données
        /// </summary>
        private MySqlConnection connection;

        /// <summary>
        /// Nom du serveur Mysql
        /// </summary>
        private string server;

        /// <summary>
        /// Nom de la base de données
        /// </summary>
        private string database;

        /// <summary>
        /// Login de l'utilisateur
        /// </summary>
        private string login;

        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        private string password;

        /// <summary>
        /// Chaîne de connexion
        /// </summary>
        private string co;

        /// <summary>
        /// Constructeur de la classe TraitementBDD
        /// </summary>
        /// <param name="server">Nom du serveur</param>
        /// <param name="database">Nom de la base de données</param>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        public TraitementBDD(string server, string database, string login, string password)
        {
            try
            {
                this.server = server;
                this.database = database;
                this.login = login;
                this.password = password;

                //On fussionne toutes les Info et on met tout dans un string,parce que Oracle ne connait pas les tableaux
                co = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + login + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(co);
            }

            catch (MySqlException ex)
            {
                Library.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Fonctions permettant de réaliser les requètes de type "select" vers la base de données.
        /// </summary>
        /// <param name="commande">Objet réalisant la requète vers la base de données</param>
        /// <returns>On retourne un MySqlDataReader (un peu comme un fetch en php)</returns>
        public MySqlDataReader interogation(string commande)
        {
            try
            {
                connection.Open();
                
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = commande;
                MySqlDataReader reader = cmd.ExecuteReader();

                return reader;
            }

            catch (MySqlException err)
            {
                //en cas d'erreur:
                //on ferme la connection

                Library.WriteErrorLog(err);
                //Si il y a un soucis les fonctions retourne NULL 
                return null;
            }
        }

        /// <summary>
        /// IUD (insert,update,delect) est une fonction qui accepte en paramètre une commande
        ///  insert update ou delect...
        /// </summary>
        /// <param name="commande">Objet réalisant la requète vers la base de données</param>
        public void execution(string commande)
        {
            try
            {
                connection.Close();
                connection.Open();
                uneTransaction = connection.BeginTransaction();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = commande;
                //execution de la requette
                cmd.ExecuteNonQuery();
                //On fait la transaction
                uneTransaction.Commit();
                //on ferme la connection
                connection.Close();
            }
            catch (MySqlException err)
            {
                //en cas d'erreur:
                //on ferme la connection
                connection.Close();
                //On inscrit dans le journal de og
                Library.WriteErrorLog(err);
                //On annule la transaction
                uneTransaction.Rollback();

            }
        }

        /// <summary>
        /// Procédure permettant de fermer la connexion en cours
        /// </summary>
        public void fermeture()
        {
            connection.Close();
        }

        /// <summary>
        /// Procédure permettant de détruire l'objet réalisant la connexion à la base de données
        /// </summary>
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
