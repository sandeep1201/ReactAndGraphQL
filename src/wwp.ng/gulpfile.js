const gulp = require('gulp');
const gulpsync = require('gulp-sync')(gulp);
const release = require('gulp-github-release');
var sass = require('gulp-dart-sass');
// sass.compiler = require('sass');

gulp.task('zip', () => {
  const argv = require('yargs').default('env', 'dist').argv;

  return zipDist(argv.env);
});

function zipDist(env) {
  const zip = require('gulp-zip');
  const argv = require('yargs').argv;

  var name = `${env}.zip`;

  if (argv.ver !== undefined) {
    name = `v${argv.ver}_${name}`;
  }

  // zip the build
  console.log(`zipping ${name}`);

  return gulp
    .src('dist/**')
    .pipe(zip(name))
    .pipe(gulp.dest('.'));
}

gulp.task('build', () => {
  const argv = require('yargs').default('env', 'dev').argv;

  buildEnv(argv.env);
});

// Because run-sequence and tasks don't handle in passing in arguments, we'll
// create custom tasks for the environments we want to build and zip.
gulp.task('build-sys', () => {
  buildEnv('sys');
});

gulp.task('build-acc', () => {
  buildEnv('acc');
});

gulp.task('build-now', () => {
  buildEnv('now');
});

gulp.task('build-prod', () => {
  buildEnv('prod');
});

gulp.task('zip-sys', () => {
  return zipDist('sys');
});

gulp.task('zip-acc', () => {
  return zipDist('acc');
});

gulp.task('zip-prod', () => {
  return zipDist('prod');
});

function buildEnv(env) {
  const exec = require('child_process').execSync;

  console.log(`building ${env}`);

  exec(`npm run build-${env}`, function(err, stdout, stderr) {
    if (err) {
      console.error(`exec error on build-sys: ${err}`);
      return;
    }
    console.log(stdout);
    console.log(stderr);
    console.log(`build ${env} complete`);
  });
}

gulp.task('deploy-now', () => {
  const exec = require('child_process').execSync;

  console.log('now starting');

  exec('cd dist', function(err, stdout, stderr) {
    if (err) {
      console.error(`exec error on cd dist: ${err}`);
      return;
    }

    console.log(stdout);
    console.log(stderr);
    console.log('cd dist complete');
  });

  exec('now', function(err2, stdout2, stderr2) {
    if (err2) {
      console.error(`exec error on now: ${err2}`);
      return;
    }
    console.log(stdout2);
    console.log(stderr2);
    console.log('now complete');
  });
});
// });

gulp.task('notify', () => {
  const PushBullet = require('pushbullet');
  var pusher = new PushBullet('o.AikWSzMfSDibneENgNEiNcf6VOvyV2In');

  // pusher.devices(function (error, response) {
  //     // response is the JSON response from the API
  //     console.log(response);
  // });

  pusher.note('ujEn4k96xj2sjzZ4YMnKUu', 'wwp.ng build', 'build is complete', function(error, response) {
    if (error) {
      console.error(`PushBullet error: ${error}`);
      return;
    }
  });
});

// In v4 of gulp, we can do this:
// gulp.task('default', gulp.series('build', 'zip'));
// But since that is pre-release, we'll use gulpsync.
gulp.task('default', gulpsync.sync(['build-sys', 'zip-sys', 'build-acc', 'zip-acc', 'build-prod', 'zip-prod', 'notify']));

gulp.task('now', gulpsync.sync(['build-now', 'deploy-now', 'notify']));

gulp.task('release', gulpsync.sync(['build-sys', 'zip-sys', 'build-acc', 'zip-acc', 'build-prod', 'zip-prod', 'git-release']));

gulp.task('git-release', () => {
  gulp.src('./sys.zip', './build.zip').pipe(
    release({
      token: '', // or you can set an env var called GITHUB_TOKEN instead
      owner: 'chujanen', // if missing, it will be extracted from manifest (the repository.url field)
      repo: 'wwp.ng', // if missing, it will be extracted from manifest (the repository.url field)
      tag: 'v1.2.4.3', // if missing, the version will be extracted from manifest and prepended by a 'v'
      name: 'v1.2.4.3', // if missing, it will be the same as the tag
      notes: '!', // if missing it will be left undefined
      draft: false, // if missing it's false
      prerelease: false, // if missing it's false
      manifest: require('./package.json') // package.json from which default values will be extracted if they're missing
    })
  );
});
