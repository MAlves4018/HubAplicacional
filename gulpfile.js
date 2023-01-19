/// <binding BeforeBuild='less, webpack' />
var gulp = require('gulp');
var less = require('gulp-less');
const webpackStream = require('webpack-stream');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
//const webpackConfig = require('./webpack.config.js');


var paths = {
    webroot: "./wwwroot/"
};

paths.less = paths.webroot + "less/*.less";
paths.compiledCss = paths.webroot + "css";
console.log(paths.less);
console.log(paths.compiledCss);

gulp.task("less", function () {
    return gulp.src(paths.less)
        .pipe(less())
        .pipe(gulp.dest(paths.compiledCss));
});



gulp.task('webpack', async function () {
    return gulp
        .src('./wwwroot/js/site.js')
        .pipe(webpackStream(
            {
                module: {
                    rules: [
                        //{ test: /\.css$/, use: ['style-loader', 'css-loader'] },
                        {
                            test: /\.css$/i,
                            use: [MiniCssExtractPlugin.loader, "css-loader"],
                        },
                    ]
                },
                plugins: [
                    new webpack.ProvidePlugin({
                        $: "jquery",
                        jQuery: "jquery"
                    }),
                    new MiniCssExtractPlugin()
                ],
                optimization: {
                    minimize: true,
                    //splitChunks: {
                    //    chunks: 'all',
                    //},
                },
                //watch: true,
                cache: {
                    type: 'filesystem',
                    compression: 'gzip',
                },
            }
        )).pipe(gulp.dest(paths.webroot + 'dist/'));
});