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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AccountManagers.Presentation.ViewModel
{
    public class UserViewModel : ViewModelBase
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
        }

        public void DisplayUsers()
        {
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

            IsEditUser = true;
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
            if(SelectedUser != null)
            {
                IsEditUser = true;
            }
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
                    IsEditUser = false;
                    MessageBox.Show("User saved.");
                }
                else
                {
                    _userRepository.UpdateUser(SelectedUser);
                    IsEditUser = false;
                    MessageBox.Show("User edited.");
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

        //public bool CanSave()
        //{
        //    IEmailValidator emailValidator = new EmailValidator();
        //    if(SelectedUser != null)
        //    {
        //        return emailValidator.IsEmailValid(SelectedUser.Email);
        //    }
        //    return false;

        //}

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
            Users.Remove(SelectedUser);
            SelectedUser = Users.FirstOrDefault();
            
            IsEditUser = false;
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
    }
}
