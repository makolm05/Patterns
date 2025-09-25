namespace SingleResponsibility.Correct
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
    }

    class Printer // right responsibility
    {
        public void PrintReport(Report report)
        {
            Console.WriteLine("Печать отчета");
            Console.WriteLine(report.Text);
        }
    }
}
