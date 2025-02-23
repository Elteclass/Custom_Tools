//Licencias necesarias para el correcto funcionamiento
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Custom_Tools_.Tools
{
    // Clase CustomPictureBox que hereda PictureBox para agregar bordes personalizados y forma circular.
    public class CustomPictureBox : PictureBox
    {
        // Propiedades privadas para el borde y estilo visual.
        private int _borderSize = 2; // Grosor del borde
        private Color _borderColor = Color.RoyalBlue; // Color primario del borde
        private Color _borderColor2 = Color.HotPink; // Color secundario del borde para efecto degradado
        private DashStyle _borderLineStyle = DashStyle.Solid; // Estilo de línea del borde
        private DashCap _borderCapStyle = DashCap.Flat; // Estilo de los extremos de la línea del borde
        private float _gradientAngle = 50F; // Ángulo del degradado de color

        // Propiedades públicas con categorías para personalización en el diseñador de Visual Studio
        [Category("Appearance")]
        public int BorderSize
        {
            get => _borderSize;
            set { _borderSize = value; Invalidate(); } // Redibuja el control al cambiar el valor
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color BorderColor2
        {
            get => _borderColor2;
            set { _borderColor2 = value; Invalidate(); }
        }

        [Category("Appearance")]
        public DashStyle BorderLineStyle
        {
            get => _borderLineStyle;
            set { _borderLineStyle = value; Invalidate(); }
        }

        [Category("Appearance")]
        public DashCap BorderCapStyle
        {
            get => _borderCapStyle;
            set { _borderCapStyle = value; Invalidate(); }
        }

        [Category("Appearance")]
        public float GradientAngle
        {
            get => _gradientAngle;
            set { _gradientAngle = value; Invalidate(); }
        }

        // Constructor de la clase
        public CustomPictureBox()
        {
            Size = new Size(100, 100); // Tamaño predeterminado
            SizeMode = PictureBoxSizeMode.StretchImage; // Ajusta la imagen dentro del control
        }

        // Método que se ejecuta cuando se redimensiona el control para mantener la forma cuadrada
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(Width, Width); // Mantiene el mismo ancho y alto para que la imagen sea circular
            Invalidate(); // Fuerza el redibujado del control
        }

        // Método que se ejecuta cuando el control necesita ser redibujado
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            var graph = pe.Graphics;
            graph.SmoothingMode = SmoothingMode.AntiAlias; // Suaviza los bordes

            // Definir rectángulos para el borde y suavizado
            var rectContourSmooth = Rectangle.Inflate(this.ClientRectangle, -1, -1);
            var rectBorder = Rectangle.Inflate(rectContourSmooth, -_borderSize, -_borderSize);
            var smoothSize = _borderSize > 0 ? _borderSize * 3 : 1;

            // Crear herramientas gráficas necesarias
            using (var borderGColor = new LinearGradientBrush(rectBorder, _borderColor, _borderColor2, _gradientAngle))
            using (var pathRegion = new GraphicsPath())
            using (var penSmooth = new Pen(this.Parent?.BackColor ?? Color.Transparent, smoothSize))
            using (var penBorder = new Pen(borderGColor, _borderSize))
            {
                penBorder.DashStyle = _borderLineStyle;
                penBorder.DashCap = _borderCapStyle;
                pathRegion.AddEllipse(rectContourSmooth);
                this.Region = new Region(pathRegion); // Establece la región circular

                // Dibujar el borde suavizado y el borde principal si el tamaño del borde es mayor a 0
                graph.DrawEllipse(penSmooth, rectContourSmooth);
                if (_borderSize > 0)
                    graph.DrawEllipse(penBorder, rectBorder);
            }
        }
    }
}