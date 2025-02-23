//Librerias necesarias para su correcto funcionamiento
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Custompanel
{
    public class RoundedPanel : Panel// Clase roundedPanel que hereda todas las funcionalidades de un Panel
    {
        // Valores predefinidos
        private int _cornerRadius = 15;
        private Color _borderColor = Color.Gray;
        private int _borderSize = 1;

        // Descripciones en el cuadro de herramientas
        [Browsable(true)]
        [Category("Appearance")]
        [Description("El radio de las esquinas redondeadas del panel.")]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = Math.Max(0, value);
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("El color del borde del panel.")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("El grosor del borde del panel.")]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = Math.Max(0, value);
                this.Invalidate();
            }
        }

        public RoundedPanel()
        {
            this.DoubleBuffered = true; // Habilita el doble buffer para evitar parpadeo
            this.BackColor = Color.White; // Fondo por defecto
            this.ForeColor = Color.Black; // Color de texto por defecto
        }

        protected override void OnPaint(PaintEventArgs e)// Metodo que se llama cada vez que el control necesita ser redibujado
        {
            base.OnPaint(e);

            // Configuración del suavizado para esquinas más nítidas
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Se ajusta las esquinas si el panel es muy pequeño
            int radius = _cornerRadius;
            if (this.Width < radius * 2 || this.Height < radius * 2)
            {
                radius = Math.Min(this.Width / 2, this.Height / 2);
            }

            // Se crea la ruta con esquinas redondeadas
            using (GraphicsPath path = new GraphicsPath())
            {
                // Se ajustan las coordenadas para esquinas redondas nitidas
                int borderOffset = _borderSize / 2;
                Rectangle rect = new Rectangle(borderOffset, borderOffset, this.Width - _borderSize, this.Height - _borderSize);

                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);

                // Se dibuja el fondo del panel
                using (Brush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Se dibuja el borde solo si el valor es mayor a 0
                if (_borderSize > 0)
                {
                    using (Pen borderPen = new Pen(_borderColor, _borderSize))
                    {
                        borderPen.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawPath(borderPen, path);
                    }
                }
            }
        }
    }
}