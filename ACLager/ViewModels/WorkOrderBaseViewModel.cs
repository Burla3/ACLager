﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels{
    public class WorkOrderBaseViewModel : BaseViewModel{

        public WorkOrderBaseViewModel() : base() {

        }

        public WorkOrderBaseViewModel(IEnumerable<WorkOrderItemTypePair> workOrderItemTypePairs, WorkOrder workorder, IEnumerable<WorkOrderItemGroup> workorderitems, ItemType itemType) : this() {
            WorkOrderItemTypePairs = workOrderItemTypePairs;
            Workorder = workorder;
            Workorderitems = workorderitems;
            ItemType = itemType;
        }

       
        public IEnumerable<WorkOrderItemGroup> Workorderitems { get; set; }
        public WorkOrder Workorder { get; set; }
        public IEnumerable<WorkOrderItemTypePair> WorkOrderItemTypePairs { get; set; }
        public ItemType ItemType { get; set; }     
    }
}