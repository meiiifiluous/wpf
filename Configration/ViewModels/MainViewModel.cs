using Configration.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Configration.ViewModels
{
    class MainViewModel
    {
        public List<DeviceModel> DeviceList { get; set; } = new List<DeviceModel>();
        public List<DeviceStateModel> StateList { get; set; } = new List<DeviceStateModel>();
        public ObservableCollection<MessageModel> MessageList { get; set; } = new ObservableCollection<MessageModel>();
        public MainViewModel()
        {
            for (int i = 0; i < 5; i++)
            {

                // 模拟数据  
                DeviceList.Add(new DeviceModel()
                {
                    DeviceNum = "AB123123",
                    StartTime = "10/11",
                    Power = "500KWH",
                    WorkTime = "1000min"
                });

            }

            StateList.Add(new DeviceStateModel
            {
                Icon = "/Asserts/Images/a1.png",
                Header = "保养中",
                Value = 12.3
            });
            StateList.Add(new DeviceStateModel
            {
                Icon = "/Asserts/Images/a2.png",
                Header = "待检验",
                Value = 43.6
            });
            StateList.Add(new DeviceStateModel
            {
                Icon = "/Asserts/Images/a3.png",
                Header = "已停用",
                Value = 10.5
            });
            StateList.Add(new DeviceStateModel
            {
                Icon = "/Asserts/Images/a1.png",
                Header = "保养中",
                Value = 12.3
            });

            for (int i = 0; i < 15; i++)
            {
                MessageList.Add(new MessageModel()
                {
                    Message = "报警消息，报警消息，报警消息",
                    Date = "07/01"
                });
            }
        }
    }
}
