﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Task7ORM.Interfaces;
using Task7ORM.Models;

namespace Task7ORM
{
    /// <summary>
    /// Repository class which represents realization of CRUD queries for Exams table
    /// </summary>
    public class SubjectsOfGroupsRepository : IRepository<SubjectsOfGroup>
    {
        /// <summary>
        /// Field for storage database context object
        /// </summary>
        private DataContext dataContext;
        private DbContext dbContext;
        private Table<SubjectsOfGroup> table;

        /// <summary>
        /// Construtor which get dataContext parametres
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="dbContext"></param>
        public SubjectsOfGroupsRepository(DataContext dataContext, DbContext dbContext)
        {
            this.dataContext = dataContext;
            this.dbContext = dbContext;
            this.table = dataContext.GetTable<SubjectsOfGroup>();
        }

        /// <summary>
        /// Property for access to dbContext field value
        /// </summary>
        public DataContext DataContext
        {
            get
            {
                return dataContext;
            }
        }

        /// <summary>
        /// Property for access to dbContext field value
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return dbContext;
            }
        }

        /// <summary>
        /// Method for insert entity into the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Create(SubjectsOfGroup model)
        {
            try
            {
                model.Id = GetUniqueKey();
                table.InsertOnSubmit(model);
                dataContext.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }        
        }

        /// <summary>
        /// Method for delete entity from the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(SubjectsOfGroup model)
        {
            try
            {
                table.DeleteOnSubmit(model);
                dataContext.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Method for select all entities from the database
        /// </summary>
        /// <returns></returns>
        public List<SubjectsOfGroup> GetAll()
        {
            List<SubjectsOfGroup> subjectsOfGroups = table.ToList();
            List<int> groupsIds = subjectsOfGroups.Select(o => o.GroupId).ToList();
            List<int> subjectsIds = subjectsOfGroups.Select(o => o.SubjectId).ToList();

            List<Group> groups = dbContext.GroupsRepository
                                          .GetAll()
                                          .Where(o => groupsIds.Contains(o.Id))
                                          .ToList();

            List<Subject> subjects = dbContext.SubjectsRepository
                                              .GetAll()
                                              .Where(o => subjectsIds.Contains(o.Id))
                                              .ToList();
            foreach (SubjectsOfGroup subjectsOfGroup in subjectsOfGroups)
            {
                Group group = groups.FirstOrDefault(o => o.Id == subjectsOfGroup.GroupId);
                Subject subject = subjects.FirstOrDefault(o => o.Id == subjectsOfGroup.SubjectId);
                subjectsOfGroup.Group = group;
                subjectsOfGroup.Subject = subject;
            }

            return subjectsOfGroups;
        }

        /// <summary>
        /// Method for select entity from the database by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubjectsOfGroup GetById(int id)
        {
            SubjectsOfGroup subjectsOfGroup = table.FirstOrDefault(o => o.Id == id);
            subjectsOfGroup.Group = dataContext.GetTable<Group>()
                                               .FirstOrDefault(o => o.Id == subjectsOfGroup.GroupId);
            subjectsOfGroup.Subject = dataContext.GetTable<Subject>()
                                                 .FirstOrDefault(o => o.Id == subjectsOfGroup.SubjectId);
            return subjectsOfGroup;
        }

        /// <summary>
        /// Method for update entity in the database
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            try
            {
                dataContext.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Helper method for generate unique key (id)
        /// </summary>
        /// <returns></returns>
        public int GetUniqueKey()
        {
            List<int> keys = table.Select(o => o.Id).ToList();
            int newId = 0;
            while (true)
            {
                if (keys.Contains(newId))
                {
                    newId++;
                }
                else
                {
                    break;
                }
            }
            return newId;
        }
    }
}
