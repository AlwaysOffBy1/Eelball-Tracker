using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EELBALL_TRACKER.ViewModels
{
    internal class VMLeaderboard : INotifyPropertyChanged
    {
        public Point pointFirstPlace = new Point(1,1);
        public Point pointLastPlace = new Point(100, 100);
        public PathGeometry pathGeometry;
        public event PropertyChangedEventHandler PropertyChanged;

        public VMLeaderboard()
        {
            this.pathGeometry = MoveFromTo(new System.Windows.Point(0,0), new System.Windows.Point(100,100));
        }
        public PathGeometry MoveFromTo(System.Windows.Point from, System.Windows.Point to)
        {
            PathGeometry pg = new PathGeometry();
            PathFigure f = new PathFigure();
            f.StartPoint = from;
            PolyBezierSegment pb = new PolyBezierSegment();
            pb.Points.Add(new System.Windows.Point((int)(from.X + 50), ((int)from.Y)));
            pb.Points.Add(new System.Windows.Point((int)(from.X - 50), ((int)from.Y - 50)));
            f.Segments.Add(pb);
            pg.Figures.Add(f);
            return pg;
            
        }
    }
}
