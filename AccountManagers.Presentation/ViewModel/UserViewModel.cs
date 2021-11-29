using AccountManagers.Common.Utils;
using AccountManagers.DataAccess.UserRepository;
using AccountManagers.Models.User;
using AccountManagers.Presentation.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AccountManagers.Presentation.ViewModel
{
    public class UserViewModel : ViewModelBase, IDataErrorInfo
    {
        public ObservableCollection<User> Users { get; set; }
        private IUserRepository _userRepository;

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");              
            }
        }
        public User UserBeforeEdit { get; set; }

        private bool _isViewUsers;
        public bool IsViewUsers
        {
            get { return _isViewUsers; }
            set { 
                _isViewUsers = value;
                OnPropertyChanged("IsViewUsers");
            }
        }

        private bool _isEditUser;
        public bool IsEditUser
        {
            get { return _isEditUser; }
            set
            {
                _isEditUser = value;
                OnPropertyChanged("IsEditUser");
            }
        }

        public UserViewModel()
        {
            _userRepository = new UserRepository();
            Users = new ObservableCollection<User>();
            DisplayUsers();
            IsViewUsers = true;
            SelectedUser = Users.FirstOrDefault();
        }

        private void DisplayUsers()
        {
            //var users = await Task.Run(() => _userRepository.GetAll());
            var users = _userRepository.GetAll();

            Users.Clear();

            foreach (var user in users)
            {
                Users.Add(user);
            }          
        }

        private ICommand _deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                if (_deleteUserCommand == null)
                {
                    _deleteUserCommand = new RelayCommand(param => DeleteUser(), null);
                }
                return _deleteUserCommand;
            }
        }
        private void DeleteUser()
        {
            if (MessageBox.Show("Are you sure that you want to delete the user?", "User", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _userRepository.DeleteUser(SelectedUser.Id);
                    MessageBox.Show("User deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error ocured." + ex.Message);
                }
                finally
                {
                    DisplayUsers();
                }
            }
        }

        private ICommand _addUserCommand;
        public ICommand AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new RelayCommand(param => AddUser(), null);
                }
                return _addUserCommand;
            }
        }
        public void AddUser()
        {
            User user = new User();
            Users.Add(user);
            SelectedUser = user;

            SwitchEditListView();
        }

        private ICommand _editUserCommand;
        public ICommand EditUserCommand
        {
            get
            {
                if (_editUserCommand == null)
                {
                    _editUserCommand = new RelayCommand(param => EditUser(), null);
                }
                return _editUserCommand;
            }
        }

        public void EditUser()
        {
            if (SelectedUser != null)
            {               
                SwitchEditListView();
            }

            //UserBeforeEdit = new User
            //{
            //    Id = SelectedUser.Id,
            //    Name = SelectedUser.Name,
            //    Email = SelectedUser.Email,
            //    CNP = SelectedUser.CNP,
            //    NoOfClients = SelectedUser.NoOfClients
            //};

        }

        private ICommand _saveUserCommand;
        public ICommand SaveUserCommand
        {
            get
            {
                if (_saveUserCommand == null)
                {
                    _saveUserCommand = new RelayCommand(param => SaveUser(), null);
                }
                return _saveUserCommand;
            }
        }
        public void SaveUser()
        {
            try
            {
                if (SelectedUser.Id == 0)
                {
                    _userRepository.InsertUser(SelectedUser);                    
                    MessageBox.Show("User saved.");
                    SwitchEditListView();
                }
                else
                {
                    _userRepository.UpdateUser(SelectedUser);                   
                    MessageBox.Show("User edited.");
                    SwitchEditListView();
                    //UserBeforeEdit = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured" + ex.Message);
            }
            finally
            {
                DisplayUsers();
            }

        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(param => CancelUser(), null);
                }
                return _cancelCommand;
            }
        }

        public void CancelUser()
        {
            if(SelectedUser.Id == 0)
            {
                Users.Remove(SelectedUser);                
            }
            //else
            //{
            //    SelectedUser = new User
            //    {
            //        Id = UserBeforeEdit.Id,
            //        Name = UserBeforeEdit.Name,
            //        Email = UserBeforeEdit.Email,
            //        CNP = UserBeforeEdit.CNP,
            //        NoOfClients = UserBeforeEdit.NoOfClients
            //    };                
            //}
            //UserBeforeEdit = null;

            SwitchEditListView();
            DisplayUsers();
            
            SelectedUser = Users.FirstOrDefault();  
        }

        private ICommand _exportToCSVCommand;
        public ICommand ExportToCSVCommand
        {
            get
            {
                if (_exportToCSVCommand == null)
                {
                    _exportToCSVCommand = new RelayCommand(param => ExportToCSV(), null);
                }
                return _exportToCSVCommand;
            }
        }

        private void ExportToCSV()
        {
            var users = _userRepository.GetAll();
            IUsersOutput userOutput = new UsersCSVWriter();
            userOutput.WriteFile(users);
            MessageBox.Show("Exported succesfully.");
        }

        private void SwitchEditListView()
        {
            IsViewUsers = !IsViewUsers;
            IsEditUser = !IsEditUser;
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(SelectedUser.Name):
                        if (string.IsNullOrEmpty(SelectedUser.Name))
                        {
                            error = "Name cannot be empty";
                        }
                        break;
                    case nameof(SelectedUser.Email):
                        IEmailValidator emailValidator = new EmailValidator();
                        var result = emailValidator.IsEmailValid(SelectedUser.Email);
                        if (result == false)
                        {
                            error = "Please enter a valid email address.";
                        }
                        break;
                    case nameof(SelectedUser.CNP):
                        if (SelectedUser.CNP.Length != 10)
                        {
                            error = "CNP must have 10 characters";
                        }
                        break;
                    default:
                        break;
                }

                return error;
            }
        }
        public string Error => string.Empty;
    }
}
