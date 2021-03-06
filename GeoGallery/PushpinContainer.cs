﻿using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps;
using System.Collections.Generic;


namespace GeoGallery
{
  /// <summary>
  /// A container for one or more pushpins at a given screen coordinate.
  /// </summary>
  public class PushpinContainer
  {
    private List<Pushpin> _pushpins = new List<Pushpin>();

    /// <summary>
    /// Creates a container for the given pushpin
    /// </summary>
    public PushpinContainer(Pushpin pushpin, Point location)
    {
      _pushpins.Add(pushpin);
      ScreenLocation = location;
    }

    /// <summary>
    /// Adds the pins from the given container
    /// </summary>
    public void Merge(PushpinContainer pinContainer)
    {
      foreach (var pin in pinContainer._pushpins)
      {
        _pushpins.Add(pin);
      }
    }

    /// <summary>
    /// Gets or sets the current screen location of this container
    /// </summary>
    public Point ScreenLocation { get; private set; }

    /// <summary>
    /// Gets the visual representation of the contents of this container. If it is 
    /// a single pushpin, the pushpin itself is returned. If multiple pushpins are present
    /// a pushpin with the given clusterTemplate is returned.
    /// </summary>
    public FrameworkElement GetElement(DataTemplate clusterTemplate)
    {
      if (_pushpins.Count == 1)
      {
        return _pushpins[0];       
          
          
      }
      else
      {
          Pushpin pp = new Pushpin()
        {
          Location = _pushpins.First().Location,
          Content = /*_pushpins.Select(pin => pin.DataContext).Count().ToString()*/_pushpins.Count.ToString(),
          ContentTemplate = clusterTemplate,   
          Tag =  _pushpins,
          Background = new SolidColorBrush(Colors.Red)          
           
        };
          
          pp.MouseLeftButtonUp += (s, e) => { 
                                              Pushpin p  = (Pushpin)s;
                                                List<Pushpin> lp = (List<Pushpin>)p.Tag;
                                              MessageBox.Show(lp.Count().ToString());
          };
          return pp;
      }
     
    }
    
  }
}
