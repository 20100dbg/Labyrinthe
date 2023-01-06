using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Labyrinthe
{
    public partial class Form1 : Form
    {
        const int ID_VIDE = 0;
        const int ID_MUR = 1;
        const int ID_DEPART = 2;
        const int ID_ARRIVEE = 3;
        const int ID_CHEMINPRIS = 4;

        int tailleGrille = 9;
        int taillePanel = 10;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string f in Directory.GetFiles(Application.StartupPath,"*.txt"))
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(f));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            int[][] grille = ReadFromFile(listBox1.SelectedItem.ToString() + ".txt");

            tailleGrille = grille.Length;
            (bool found, List<int> path) = PathFinder(grille);
            
            grille = PrepareDraw(grille);

            Thread thread = new Thread(() => Draw(grille, path));
            thread.Start();
        }


        private int[][] PrepareDraw(int[][] grille)
        {
            int tailleConteneur = tailleGrille * 16;

            fPanel.Size = new Size(tailleConteneur, tailleConteneur);

            for (int i = 0; i < tailleGrille * tailleGrille; i++)
            {
                Panel p = new Panel();
                p.Size = new Size(taillePanel, taillePanel);
                fPanel.Controls.Add(p);
            }

            for (int i = 0; i < grille.Length; i++)
            {
                for (int j = 0; j < grille.Length; j++)
                {
                    int idx = i * grille.Length + j;
                    if (grille[i][j] == ID_VIDE || grille[i][j] == ID_CHEMINPRIS) fPanel.Controls[idx].BackColor = Color.White;
                    else if (grille[i][j] == ID_MUR) fPanel.Controls[idx].BackColor = Color.Black;
                    else if (grille[i][j] == ID_ARRIVEE) fPanel.Controls[idx].BackColor = Color.Gold;
                }
            }

            return grille;
        }

        private void Draw(int[][] grille, List<int> path)
        {
            fPanel.Invoke((MethodInvoker)delegate {
                fPanel.Controls[path[0]].BackColor = Color.DarkBlue;
            });

            for (int j = 1; j < path.Count - 1; j++)
            {
                Thread.Sleep(300);

                fPanel.Invoke((MethodInvoker)delegate {
                    fPanel.Controls[path[j]].BackColor = Color.Green;    
                });
            }

        }


        private (bool found, List<int> path) PathFinder(int[][] grille)
        {
            (int iDepart, int jDepart) = TrouverCar(grille, ID_DEPART);
            (int iArrivee, int jArrivee) = TrouverCar(grille, ID_ARRIVEE);

            if (iDepart == -1) return (false, new List<int>());
            if (iArrivee == -1) return (false, new List<int> { });

            (bool found, List<int> path) = Explorer(grille, iDepart, jDepart, new List<int>());

            return (found, path);
        }

        private (bool, List<int>) Explorer(int[][] grille, int i, int j, List<int> path) //List<int[]> path)
        {
            path.Add(i * grille.Length + j);
            if (grille[i][j] == ID_ARRIVEE) return (true, path);

            //ne pas aller vers la position précédente ?
            grille[i][j] = ID_CHEMINPRIS;

            if (j < grille.Length - 1 && CaseValide(grille[i][j + 1]))
            {
                (bool valid, List<int> mypath) = Explorer(grille, i, j + 1, new List<int>(path));
                if (valid) return (true, mypath);
            }


            if (i > 0 && CaseValide(grille[i - 1][j]))
            {
                (bool valid, List<int> mypath) = Explorer(grille, i - 1, j, new List<int>(path));
                if (valid) return (true, mypath);
            }

            
            if (i < grille.Length - 1 && CaseValide(grille[i + 1][j]))
            {
                (bool valid, List<int> mypath) = Explorer(grille, i + 1, j, new List<int>(path));
                if (valid) return (true, mypath);
            }

            if (j > 0 && CaseValide(grille[i][j - 1]))
            {
                (bool valid, List<int> mypath) = Explorer(grille, i, j - 1, new List<int>(path));
                if (valid) return (true, mypath);
            }

            return (false, path);
        }


        private (int, int) TrouverCar(int[][] grille, int aTrouver)
        {
            for (int i = 0; i < grille.Length; i++)
            {
                for (int j = 0; j < grille.Length; j++)
                {
                    if (grille[i][j] == aTrouver) return (i, j);
                }
            }
            return (-1, -1);
        }

        private bool CaseValide(int val)
        {
            return (val == ID_VIDE || val == ID_ARRIVEE);
        }


        public int[][] ReadFromFile(string filename)
        {
            List<int[]> tab = new List<int[]>();

            using (StreamReader sr = new StreamReader(filename))
            {
                string? line;

                while ((line = sr.ReadLine()) != null)
                {
                    int[] tabint = new int[line.Length];

                    for (int i = 0; i < line.Length; i++)
                        tabint[i] = int.Parse(line[i].ToString());

                    tab.Add(tabint);
                }
            }

            return tab.ToArray();
        }

    }
}