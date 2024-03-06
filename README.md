# SurfaceLighting
WinForms application used to animate surface lighting under the influence of various manipulations, e.g.
- background change,
- giving curvature (Bezier curve)
- variable position of the light source
- manipulating the coefficients of the formula determining the color of the surface at a given point
- applying NormalMap to the surface

<img width="839" alt="image" src="https://github.com/werag55/SurfaceLighting/assets/147431062/a130ec9e-135f-4a3a-b5ba-fee1ae290ac0">

**Instruction**

We enter non-integer values using TextBoxes using a comma (not a dot).

The transformed triangle grid is drawn after modifying the alpha/beta angles with the slider.

The "Bezier surface's control points" checkbox is responsible for the visibility of circles symbolizing control points.
After clicking on one of the control points, the TrackBar "Z coordinate of the selected control point" and the corresponding TextBox become active, which can be used to manipulate the height of a given control point (accepted values: 0 - 2).

The "Triangle grid" checkbox is responsible for the visibility of the triangle grid. TrackBar and TextBox below - for the accuracy of this grid (setting the n value on the TrackBar/TextBox control corresponds to a grid with 2*n*n triangles) (accepted values: 3 - 30).

The next three TrackBars (and the adjacent TextBoxes) correspond to the coefficients kd, ks and m from the formula for I - pixel color (accepted values: 0 - 1, 0 - 1, 1 - 100, respectively):
$I = k_d * I_L * I_O * cos(angle(N,L)) + k_s * I_L * I_O * cos^m(angle(V,R))$
 
In the "Color of the object:" section, you can use RadioButtons to select a background from: "Solid color" (the solid background color you are interested in can be selected using the button next to it, which displays the current background color) and "Image" (after selecting it, the adjacent button is activated "Select..." allows you to change the current photo displayed in the background to another one selected from your computer's files).
 
Use the color button next to the "Color of the light:" label to select the light color.
The "Light movement" checkbox is responsible for resuming/stopping the animation of the light source's movement in a spiral with a constant height.
Below there is a TrackBar and an adjacent TextBox that allows you to modify the height of the light source (accepted values: 2 - 10).

The "NormalMap" checkbox is responsible for loading the normalmap. Once selected, the "Select..." button is activated, allowing you to load another normalmap from the files.
