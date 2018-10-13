using System;

namespace ZBYGate_201810111245.Action
{
    class ContainerAc
    {
        #region//委托
        public Action<int> LinkAction;
        public Action<int> AbortAction;
        public Action<int> LastRAction;
        #endregion

        #region//对象
        private Container.Container _Container = new Container.Container();
        #endregion

        public ContainerAc()
        {          
            LinkAction  += _Container.LinkC;
            AbortAction += _Container.CloseC;
            LastRAction += _Container.LastR;
        }

        public void Message(string message)
        {

        }
    }
}
