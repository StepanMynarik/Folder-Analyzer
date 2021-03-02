/**
 * Based on https://github.com/jonschlinkert/delete-empty/blob/master/index.js
 * Praise Jon Schlinkert
 */

import fs from 'fs';
import util from 'util';
import path from 'path';
import rimraf from 'rimraf';
import startsWith from 'path-starts-with';
import colors from 'ansi-colors';
const readdir = util.promisify(fs.readdir);

/**
 * Helpers
 */

const GARBAGE_REGEX = /(?:Thumbs\.db|\.DS_Store)$/i;
const isGarbageFile = (file: string, regex = GARBAGE_REGEX) => regex.test(file);
const filterGarbage = (file: string, regex: RegExp) => !isGarbageFile(file, regex);
const isValidDir = (cwd: string, dir: string, empty: string | string[]) => {
  return !empty.includes(dir) && startsWith(dir, cwd) && isDirectory(dir);
};

const deleteDir = async (dirname: string, options: { dryRun: boolean }) => {
  if (options.dryRun !== true) {
    return new Promise((resolve, reject) => {
      rimraf(dirname, { ...options, glob: false }, (err: any) => {
        if (err) {
          reject(err);
        } else {
          resolve();
        }
      });
    })
  }
};

const deleteDirSync = (dirname: string, options: { dryRun: boolean }) => {
  if (options.dryRun !== true) {
    return rimraf.sync(dirname, { ...options, glob: false });
  }
};

const deleteEmpty = (cwd: string, options: {}, cb: (arg0: any, arg1: any) => any) => {
  if (typeof options === 'function') {
    cb = options;
    options = null;
  }

  if (typeof cb === 'function') {
    return deleteEmpty(cwd, options)
      .then((res: any) => cb(null, res))
      .catch(cb);
  }

  const opts = options || {};
  const dirname = path.resolve(cwd);
  const onDirectory = opts.onDirectory || (() => {});
  const empty: string[] = [];

  const remove = async (filepath: string): Promise<string[]> => {
    let dir = path.resolve(filepath);

    if (!isValidDir(cwd, dir, empty)) return;
    onDirectory(dir);

    let files = await readdir(dir);

    if (isEmpty(dir, files, empty, opts)) {
      empty.push(dir);

      await deleteDir(dir, opts);

      if (opts.verbose === true) {
        console.log(colors.red('Deleted:'), path.relative(cwd, dir));
      }

      if (typeof opts.onDelete === 'function') {
        await opts.onDelete(dir);
      }

      return remove(path.dirname(dir));
    }

    for (const file of files) {
      await remove(path.join(dir, file));
    }

    return empty;
  };

  return remove(dirname);
};

deleteEmpty.sync = (cwd: string, options: {}) => {
  if (typeof cwd !== 'string') {
    throw new TypeError('expected the first argument to be a string');
  }

  const opts = options || {};
  const dirname = path.resolve(cwd);
  const deleted = [];
  const empty: string[] = [];

  const remove = (filepath: string): string[] => {
    let dir = path.resolve(filepath);

    if (!isValidDir(cwd, dir, empty)) {
      return empty;
    }

    let files = fs.readdirSync(dir);

    if (isEmpty(dir, files, empty, opts)) {
      empty.push(dir);

      deleteDirSync(dir, opts);

      if (opts.verbose === true) {
        console.log(colors.red('Deleted:'), path.relative(cwd, dir));
      }

      if (typeof opts.onDelete === 'function') {
        opts.onDelete(dir);
      }

      return remove(path.dirname(dir));
    }

    for (let filepath of files) {
      remove(path.join(dir, filepath));
    }
    return empty;
  };

  remove(dirname);
  return empty;
};

/**
 * Return true if the given `files` array has zero length or only
 * includes unwanted files.
 */

const isEmpty = (dir: string, files: string[], empty: string | any[], options = {}) => {
  let filter = options.filter || filterGarbage;
  let regex = options.junkRegex;

  for (let basename of files) {
    let filepath = path.join(dir, basename);

    if (!(options.dryRun && empty.includes(filepath)) && filter(filepath, regex) === true) {
      return false;
    }
  }
  return true;
};

/**
 * Returns true if the given filepath exists and is a directory
 */

const isDirectory = (dir: fs.PathLike) => {
  try {
    return fs.statSync(dir).isDirectory();
  } catch (err) { /* do nothing */ }
  return false;
};

/**
 * Expose deleteEmpty
 */

module.exports = deleteEmpty;