namespace SingleResponsibility.Wrong
{
    class Report
    {
        public string Text { get; set; } = "";
        public void GoToFirstPage() =>
            Console.WriteLine("Переход к первой странице");

        public void GoToLastPage() =>
            Console.WriteLine("Переход к последней странице");

        public void GoToPage(int pageNumber) =>
            Console.WriteLine($"Переход к странице {pageNumber}");


        public void Print() // <-- wrong responsibility
        {
            Console.WriteLine("Печать отчета");
            Console.WriteLine(Text);
        }
    }
}
