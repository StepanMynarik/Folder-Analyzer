const rules = require('./webpack.rules');
const plugins = require('./webpack.plugins');

rules.push({
  test: /\.css$/,
  use: [{ loader: 'style-loader' }, { loader: 'css-loader' }],
},
{
  test: /\.(png|jpe?g|gif|svg|webp)$/,
  use: [
    {
      loader: 'url-loader',
      options: {
        limit: 1024,
        name: 'img/[name].[hash:7].[ext]'
      },
    },
  ],
},
{
  test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
  use: [
    {
      loader: 'url-loader',
      options: {
        limit: 1024,
        name: 'fonts/[name].[hash:7].[ext]'
      },
    },
  ],
});

module.exports = {
  module: {
    rules,
  },
  plugins: plugins,
  resolve: {
    extensions: ['.js', '.ts', '.jsx', '.tsx', '.css']
  },
};
