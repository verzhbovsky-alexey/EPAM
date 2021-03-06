﻿using System.Data.Linq.Mapping;

namespace Task7ORM.Models
{
    /// <summary>
    /// Class which represent model of Group of database
    /// </summary>
    [Table(Name = "Groups")]
    public class Group
    {
        /// <summary>
        /// Property which storage primary key of group
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = false)]
        public int Id { get; set; }

        /// <summary>
        /// Property which storage name of group
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Property which storage speciality id
        /// </summary>
        [Column(Name = "SpecialityId")]
        public int SpecialityId { get; set; }

        /// <summary>
        /// Property which storage object of Speciality
        /// </summary>
        public Speciality Speciality { get; set; }
    }
}
