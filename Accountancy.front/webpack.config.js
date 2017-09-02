
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
      }
    ]
  },

  devtool: "source-map"
};

module.exports = config;