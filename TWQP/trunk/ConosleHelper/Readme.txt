
һС��ʾ����

static void Main(string[] args)
{
    Console.Title = "xxxxxxxxxxxxxxxxxx";

    var menu = new Menu(true, null, MenuHandler);
}

static void MenuHandler(object sender, EventConsoleUIArgs e)
{
    var c = (Menu)sender;
    var m = new MenuItem(c, null, "���˵�", "", ConsoleKey.Escape, null);
    c.Current = c.Root = m;

    var m0 = m.Add("�˳�", "", ConsoleKey.Escape, (o, ea)=>{ c.Escape(); });
    var m1 = m.Add("�Ӳ˵�1", "", ConsoleKey.D1, (o, ea)=>{ m.Writer.WL("xxx"); });
    var m2 = m.Add("�Ӳ˵�2", "", ConsoleKey.D2, null);

    m2.Action += new EventMenuItemActionHandler(m1_Action);
...