/// <binding BeforeBuild='clean, sass' Clean='clean, sass' />
"use strict";

const gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sass = require('gulp-sass');


const paths = {    
    webroot: "./wwwroot/"
};

paths.scss = paths.webroot + "scss/**/*.scss";
paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.cssFolder = paths.webroot + "css";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", done => rimraf(paths.concatJsDest, done));
gulp.task("clean:css", done => rimraf(paths.concatCssDest, done));
gulp.task("clean", gulp.series(["clean:js", "clean:css"]));

gulp.task('sass', function () {
    return gulp.src(paths.scss)
        .pipe(sass()) // Converts Sass to CSS with gulp-sass
        .pipe(gulp.dest(paths.cssFolder))
});

gulp.task('sass:watch', function () {
    gulp.watch(paths.scss, ['sass']);
});

gulp.task("min:css", () => {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task('min:css:watch', function () {
    gulp.watch(paths.css, ['min:css']);
});

gulp.task("min:js", () => {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min", gulp.series(["min:js", "min:css"]));

// A 'default' task is required by Gulp v4
gulp.task("default", gulp.series(["min"]));