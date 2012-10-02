using System;

namespace mvcweb.App
{
    public class Pager
    {
        private int pageSize;

        public Pager(int total, int pageNum, int pageSize)
        {
            Total = total;
            PageNum = pageNum;
            PageSize = pageSize;
        }

        public int Total { get; set; }
        public int PageNum { get; set; }

        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                if (Total < value || value < 1)
                    pageSize = Total;

                else
                    pageSize = value;
            }
        }

        public string GetItemMessage()
        {
            return Utils.GetItemMessage(Total, PageNum, PageSize);
        }

        public int LowerBound
        {
            get
            {
                return (PageNum - 1) * PageSize;
            }
        }

        public int UpperBound
        {
            get
            {
                int upperBound = PageNum * PageSize;
                if (Total < upperBound)
                    upperBound = Total;

                return upperBound;
            }
        }

        public bool HasNext
        {
            get
            {
                return (Total > UpperBound ? true : false);
            }
        }

        public bool HasPrev
        {
            get
            {
                return (LowerBound > 0 ? true : false);
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)Total / PageSize);
            }
        }
    }
}