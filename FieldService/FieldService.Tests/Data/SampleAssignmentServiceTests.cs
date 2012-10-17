﻿//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;
using NUnit.Framework;

namespace FieldService.Tests.Data {
    [TestFixture]
    public class SampleAssignmentServiceTests {
        IAssignmentService service;

        [SetUp]
        public void SetUp ()
        {
            var task = Database.DropTables ().ContinueWith (Database.Initialize ());
            task.Wait ();

            service = new SampleAssignmentService ();
        }

        [Test]
        public void GetAssignments ()
        {
            var task = service.GetAssignmentsAsync ();

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void SaveAssignment ()
        {
            var task = service.GetAssignmentsAsync ();
            Task.WaitAll (task);
            var assignment = task.Result.FirstOrDefault ();
            assignment.Title = "New Title";
            var saveTask = service.SaveAssignment (assignment);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveAssignmentItemInsert ()
        {
            var assignmentItem = new AssignmentItem ();
            assignmentItem.Item = 1;
            assignmentItem.Assignment = 1;
            var saveTask = service.SaveAssignmentItem (assignmentItem);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveAssignmentItemUpdate ()
        {
            var assignmentItem = new AssignmentItem ();
            assignmentItem.Item = 1;
            assignmentItem.Assignment = 1;
            var saveTask = service.SaveAssignmentItem (assignmentItem);
            saveTask.Wait ();
            assignmentItem.Item = 1;
            assignmentItem.Assignment = 2;
            saveTask = service.SaveAssignmentItem (assignmentItem);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveLaborInsert ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";
            var saveTask = service.SaveLabor (labor);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveLaborUpdate ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";
            var saveTask = service.SaveLabor (labor);
            saveTask.Wait ();
            labor.Description = "New Description 2";
            saveTask = service.SaveLabor (labor);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveExpenseInsert ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";
            var saveTask = service.SaveExpense (expense);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveExpenseUpdate ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";
            var saveTask = service.SaveExpense (expense);
            saveTask.Wait ();
            expense.Description = "New Description 2";
            saveTask = service.SaveExpense (expense);
            saveTask.Wait ();
            Assert.That (saveTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void GetItems ()
        {
            var task = service.GetItemsAsync ();

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));
        }

        [Test]
        public void GetAssignmentItems ()
        {
            var assignmentTask = service.GetAssignmentsAsync ();

            Task.WaitAll (assignmentTask);

            var task = service.GetItemsForAssignmentAsync (assignmentTask.Result.First ());

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var item in task.Result) {
                Assert.That (item.Name, Is.Not.Null);
                Assert.That (item.Number, Is.Not.Null);
            }
        }

        [Test]
        public void GetAssignmentLabor ()
        {
            var assignmentTask = service.GetAssignmentsAsync ();

            Task.WaitAll (assignmentTask);

            var task = service.GetLaborForAssignmentAsync (assignmentTask.Result.First ());

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var labor in task.Result) {
                Assert.That (labor.Description, Is.Not.Null);
                Assert.That (labor.Type, Is.Not.Null);
            }
        }

        [Test]
        public void GetAssignmentExpenses ()
        {
            var assignmentTask = service.GetAssignmentsAsync ();

            Task.WaitAll (assignmentTask);

            var task = service.GetExpensesForAssignmentAsync (assignmentTask.Result.First ());

            Task.WaitAll (task);

            Assert.That (task.Result, Is.Not.Null);
            Assert.That (task.Result.Count, Is.Not.EqualTo (0));

            foreach (var expense in task.Result) {
                Assert.That (expense.Description, Is.Not.Null);
                Assert.That (expense.Category, Is.Not.Null);
            }
        }

        [Test]
        public void DeleteAssignment()
        {
            var task = service.GetAssignmentsAsync ();
            Task.WaitAll (task);
            var assignment = task.Result.FirstOrDefault ();
            assignment.Title = "New Title";
            var saveTask = service.SaveAssignment (assignment);
            saveTask.Wait ();

            var deleteTask = service.DeleteAssignment (assignment);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeleteAssignmentItem ()
        {
            var assignmentItem = new AssignmentItem ();
            var saveTask = service.SaveAssignmentItem (assignmentItem);
            saveTask.Wait ();

            var deleteTask = service.DeleteAssignmentItem (assignmentItem);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeleteLabor ()
        {
            var labor = new Labor ();
            labor.Description = "New Description";
            var saveTask = service.SaveLabor (labor);
            saveTask.Wait ();

            var deleteTask = service.DeleteLabor (labor);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void DeleteExpense ()
        {
            var expense = new Expense ();
            expense.Description = "New Description";
            var saveTask = service.SaveExpense (expense);
            saveTask.Wait ();

            var deleteTask = service.DeleteExpense (expense);
            deleteTask.Wait ();
            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }

        [Test]
        public void SaveTimerEntry ()
        {
            var task = service.SaveTimerEntry (new TimerEntry { Date = DateTime.Now.AddHours (-1) });

            task.Wait ();

            Assert.That (task.Result, Is.EqualTo (1));
        }

        [Test]
        public void UpdateTimerEntry ()
        {
            var entry = new TimerEntry { Date = DateTime.Now.AddHours (-1) };
            var task = service.SaveTimerEntry (entry);

            task.Wait ();

            entry.Date = entry.Date.AddHours (2);
            task = service.SaveTimerEntry (entry);

            Assert.That (task.Result, Is.EqualTo (1));
        }

        [Test]
        public void GetTimerEntry ()
        {
            var entry = new TimerEntry { Date = DateTime.Now.AddHours (-1) };
            var task = service.SaveTimerEntry (entry);

            task.Wait ();

            var getTask = service.GetTimerEntryAsync ();

            getTask.Wait ();
            
            var result = getTask.Result;
            Assert.That (entry.ID, Is.EqualTo (result.ID));
            Assert.That (entry.Date, Is.EqualTo (result.Date));
        }

        [Test]
        public void DeleteTimerEntry ()
        {
            var entry = new TimerEntry { Date = DateTime.Now.AddHours (-1) };
            var task = service.SaveTimerEntry (entry);

            task.Wait ();

            var deleteTask = service.DeleteTimerEntry (entry);

            deleteTask.Wait ();

            Assert.That (deleteTask.Result, Is.EqualTo (1));
        }
    }
}