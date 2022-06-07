﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfMvvmApp.Models;

namespace WpfMvvmApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //View에서 사용하기 위해서 만든 멤버 변수
        private string inFirstName;
        private string inLastName;
        private string outFullName;
        private string inEmail;
        private DateTime inDate;

        private string outFirstName;
        private string outLastName;
        private string outEmail;
        private string outDate;

        private string outAdult;
        private string outBirthday;

        //실제 사용하는 속성(Property)
        public string InFirstName
        {
            get { return inFirstName; }
            set
            {
                inFirstName = value;
                RaisePropertyChanged("InFirstName"); // 값이 바뀜 공지!!
            }
        }

        public string OutFullName
        {
            get { return outLastName + outFirstName; }
            set
            {
                outFullName = value;
                RaisePropertyChanged("OutFullName"); // 값이 바뀜 공지!!
            }
        }

        public string InLastName
        {
            get { return inLastName; }
            set
            {
                inLastName = value;
                RaisePropertyChanged("InLastName"); // 값이 바뀜 공지!!
            }
        }
        public string InEmail
        {
            get { return inEmail; }
            set
            {
                inEmail = value;
                RaisePropertyChanged("InEmail"); // 값이 바뀜 공지!!
            }
        }
        public DateTime InDate
        {
            get { return inDate; }
            set
            {
                inDate = value;
                RaisePropertyChanged("InDate"); // 값이 바뀜 공지!!
            }
        }
        public string OutFirstName
        {
            get { return outFirstName; }
            set
            {
                outFirstName = value;
                RaisePropertyChanged("OutFirstName"); // 값이 바뀜 공지!!
            }
        }
        public string OutLastName
        {
            get { return outLastName; }
            set
            {
                outLastName = value;
                RaisePropertyChanged("OutLastName"); // 값이 바뀜 공지!!
            }
        }
        public string OutEmail
        {
            get { return outEmail; }
            set
            {
                outEmail = value;
                RaisePropertyChanged("OutEmail"); // 값이 바뀜 공지!!
            }
        }
        public string OutDate
        {
            get { return outDate; }
            set
            {
                outDate = value;
                RaisePropertyChanged("OutDate"); // 값이 바뀜 공지!!
            }
        }
        public string OutAdult
        {
            get { return outAdult; }
            set
            {
                outAdult = value;
                RaisePropertyChanged("OutAdult"); // 값이 바뀜 공지!!
            }
        }
        public string OutBirthday
        {
            get { return outBirthday; }
            set
            {
                outBirthday = value;
                RaisePropertyChanged("OutBirthday"); // 값이 바뀜 공지!!
            }
        }

        //값이 전부적용되서 버튼을 활성화하기 위한 명령
        private ICommand proceedCommand;
        public ICommand ProceedCommnad
        {
            get {
                return proceedCommand ?? (
                  proceedCommand = new RelayCommand<object>(
                      o => Proceed(), o => !string.IsNullOrEmpty(InFirstName) &&
                                          !string.IsNullOrEmpty(inLastName) &&
                                          !string.IsNullOrEmpty(InEmail) &&
                                          !string.IsNullOrEmpty(InDate.ToString())
                      ));
            }
        }

        //버튼클릭시 일어나는 실제 명령의 실체
        private async void Proceed()
        {
            try
            {
                Person person = new Person(inFirstName, inLastName, inEmail, inDate);

                await Task.Run(() => OutFirstName = person.FirstName);
                await Task.Run(() => OutLastName = person.LastName);
                await Task.Run(() => OutEmail = person.Email);
                await Task.Run(() => OutDate = person.Date.ToShortDateString());
                await Task.Run(() => OutBirthday = $"{person.IsBirthday}");
                await Task.Run(() => OutAdult = $"{ person.IsAdult}");
                await Task.Run(() => OutFullName = outFullName);
                //To Do 

            }
            catch (Exception ex)
            {

                MessageBox.Show($"예외발생 {ex.Message}");
            }
        }

        public MainViewModel()
        {
            this.inDate = DateTime.Parse("1990-01-01");
        }
    }
}
