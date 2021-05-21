const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const webpack = require('webpack')

module.exports = {
    entry: {
        currentuser: './src/currentuserindex.js',
        accounting: './src/accounting.js',
        contracts: './src/contracts.js',
        wells: './src/wells.js',
        units: './src/units.js',
        administration: './src/administration.js',
        surety: './src/surety.js'
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js', 'default'],     
        }),
        new CleanWebpackPlugin(),
    ],
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/webpack')
    },
    module: {
        rules: [
            {
                test: /\.handlebars$/,
                loader: "handlebars-loader",
                options: {
                    helperDirs: path.resolve(__dirname, '..', 'src/views/helpers')
                }
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ],
            },
            {
                test: /\.(jpe?g|png|gif)$/i,
                loader: "file-loader",
                options: {
                    name: '[name].[ext]',
                    outputPath: '../assets/images/'
                    //the images will be emited to wwwroot/assets/images/ folder
                }
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    }
};