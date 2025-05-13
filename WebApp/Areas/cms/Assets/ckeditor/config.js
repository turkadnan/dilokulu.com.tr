/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.uiColor = '#E7ECF1';
    config.removePlugins = 'elementspath';
    config.resize_enabled = false;
    config.toolbarCanCollapse = false;
    config.enterMode = CKEDITOR.ENTER_BR;
    config.height = '460';
    config.width = '790';
    config.skin = 'kama';
    config.resize_enabled = false;
    config.pasteFromWordCleanupFile = 'custom';
    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;
    config.toolbar_BBasic = [['Bold', 'Italic', 'Underline', 'PasteFromWord', 'Source']];


};
