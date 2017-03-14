# BatchFeaturestoGraphics
## ESRI Add-In for ArcGIS 10.4.1

This application converts all the selected features in your map extent to graphics.

## Background
To produce maps of cartographically pleasing output, one must move around overlapping symbols on the map in layout view. With overlapping symbols at a required size, this can be a difficult task. Without editing the location of the data, the best way to tackle this problem is to convert each layer to graphics and manually move each symbol to a visible position. This technique preserves the accuracy of your dataset and ensures all your data is visible on the map.

Converting multiple layers to graphics can be a time0consuming process. In the default ArcGIS environment, you must select each layer separately and go through the event one by one. Not only is this time consuming, this process can also lead to accuracy problems, as there is no way to see if you have already converted a layer to graphics.

This tool only converts the layers in the map layout extent to graphics. Converting an entire dataset to graphics can dramatically slow down the performance of your map document. 

## Installation

Clone or download the BatchFeaturesToGraphics folder and install the Esri Add-In in the bin, Debug folder. Use the Customize Mode dialog and move the button to an existing toolbar. 

Works with shapefiles. 
