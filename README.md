This is my implementation of the alghoritm described by Étienne Jacob [here](https://necessarydisorder.wordpress.com/2019/02/20/distortion-or-smoke-effect-on-parametric-curves/).

I've used WPF and .NET Framework as a "host" for the animation generator. Most interesting files are MainWindow.xaml.cs and Shape.cs.

The final effect is rendered inside WPF application via [WriteableBitmapExt](https://github.com/reneschulte/WriteableBitmapEx) because of poor performanceof WPF' Canvas or plain WriteableBitmap.

To generate animation the [Perlin noise](https://en.wikipedia.org/wiki/Perlin_noise) is being used. I've been unable to find original author of the C# implementation.



