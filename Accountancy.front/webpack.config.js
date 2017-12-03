var webpack = require('webpack');
var path = require("path");

var config = {

  entry: ["./src/Index.tsx"],

  output: {
    publicPath: "build/",
    path: path.resolve(__dirname, "build"),
    filename: "bundle.js"
  },

  resolve: {
    extensions: [".ts", ".tsx", ".js"],
    modules: [
      path.resolve(__dirname, 'src'),
      'node_modules'
    ]
  },

  module: {
    loaders: [
      {
        test: /\.tsx?$/,
        use: 'awesome-typescript-loader',
        exclude: /node_modules/
      },
      {
         test: /\.css$/, 
         loader: "style-loader!css-loader"
      },
      {
        test: /\.scss$/,
        loaders: ['style', 'css', 'postcss', 'sass']
      }, {
        test: /\.less$/,
        loaders: ['style', 'css', 'less']
      }
    ]
  },
  plugins: [
      new webpack.ProvidePlugin({
        $: 'jquery',
        jQuery: 'jquery',
        'window.jQuery': 'jquery',
        Popper: ['popper.js', 'default'],
      }),
      new webpack.HotModuleReplacementPlugin()
  ],

  devtool: "source-map",

  devServer: {
    contentBase: './dist',
    hot: true,
    historyApiFallback: true
  },
};

module.exports = config;