/// <binding ProjectOpened='watch' />
// / <binding BeforeBuild='default' />
// Include gulp
const gulp = require('gulp');

// Include Our Plugins
const concat = require('gulp-concat');
const run = require('gulp-run');
const cleanCSS = require('gulp-clean-css');
const del = require('del');
const rename = require('gulp-rename');

// Concatenate Vendor CSS
function vendorcss() {
    return gulp
        .src([
            'node_modules/bootstrap/dist/css/bootstrap.min.css',
            'node_modules/@fortawesome/fontawesome-free/css/all.min.css',
            'node_modules/font-awesome-animation/dist/font-awesome-animation.min.css',
            'StyleSheet.css',
        ])
        .pipe(cleanCSS())
        .pipe(concat('style.min.css'))
        .pipe(gulp.dest('wwwroot/vendor'));
}


function webfonts() {
    return gulp
        .src(['node_modules/@fortawesome/fontawesome-free/webfonts/**/*'])
        .pipe(gulp.dest('wwwroot/webfonts'));
}

function images() {
    return gulp
        .src(['images/**/*'])
        .pipe(gulp.dest('wwwroot/images'));
}

function favicon() {
    return gulp
        .src(['images/favicon-32X32.png'])
        .pipe(rename('favicon.ico'))
        .pipe(gulp.dest('wwwroot'));
}

function cleanAll() {
    return del(['wwwroot/**', '!wwwroot']);
}

function vendorjs() {
    return gulp
        .src([
            'node_modules/jquery/dist/jquery.min.js',
            'node_modules/popper.js/dist/umd/popper.min.js',
            'node_modules/bootstrap/dist/js/bootstrap.min.js',          
        ])
        .pipe(concat('vendor.min.js'))
        .pipe(gulp.dest('wwwroot/vendor'));
}

function builddev() {
    return run('npm run builddev').exec();
}

function buildprod() {
    return run('npm run buildprod').exec();
}


const build = gulp.series(cleanAll, gulp.parallel(vendorcss, vendorjs, webfonts, images, favicon, builddev));
const prodbuild = gulp.series(cleanAll, gulp.parallel(vendorcss, vendorjs, webfonts, images, favicon, buildprod));

function watch() {
    return gulp
        .watch('./src/**/*', gulp.parallel(builddev));
}

exports.default = build;
exports.productionbuild = prodbuild;
exports.watch = watch;
